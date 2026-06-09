using PSG2SaveEditor.SaveFormat;

byte[] m = P2SArchive.Load(@"D:\Emulation\PS2\pcsx2-v2.7.75-windows-x64-Qt\Saves\PhantasyStarGeneration2.p2s").GetData("eeMemory.bin");
int n = m.Length;

int[] weapon={1,11,28,36,53}, head={59,66}, body={82,83}, feet={128,129}, consum={138,139,144,148,149};
int[] all={1,11,28,36,53,59,66,82,83,128,129,138,139,144,148,149};
int MAXID=149;
int[] strides={8,12,16,20,24,28,32,36,40,44,48,52,56,60,64,72,80,96,112,128,160,192,256};

bool Same(int g,int s,int[] ids,out int v){ v=m[g+ids[0]*s]; foreach(var id in ids) if(m[g+id*s]!=v) return false; return true; }

int found=0;
foreach(int s in strides){
    long limit=n-(long)(MAXID+1)*s;
    for(int g=0; g<limit; g++){
        if(m[g+s]!=m[g+11*s]) continue;
        if(!Same(g,s,weapon,out int vw)) continue;
        if(!Same(g,s,head,out int vh)) continue;
        if(!Same(g,s,body,out int vb)) continue;
        if(!Same(g,s,feet,out int vf)) continue;
        if(!Same(g,s,consum,out int vc)) continue;
        var set=new HashSet<int>{vw,vh,vb,vf,vc};
        if(set.Count!=5 || set.Any(v=>v>31)) continue;
        // REQUIRE per-item variation: some byte offset d in the record differs across items
        // (kills constant-run false positives). Find the record start window [g-s+1 .. g].
        int bestDistinct=0;
        for(int d=0; d<s; d++){
            int rb=g - (g% s);                // align record base to a stride grid through g
            var vals=new HashSet<int>();
            foreach(var id in all) vals.Add(m[rb + id*s + d]);
            if(vals.Count>bestDistinct) bestDistinct=vals.Count;
        }
        if(bestDistinct<6) continue;          // need a field with >=6 distinct values across 16 items
        Console.WriteLine($"HIT stride={s} g=0x{g:X} slotVals(W{vw} H{vh} B{vb} F{vf} C{vc}) maxFieldDistinct={bestDistinct}");
        if(++found>=30){ Console.WriteLine("...stop"); return; }
    }
}
Console.WriteLine($"\ndone, {found} hit(s)");
