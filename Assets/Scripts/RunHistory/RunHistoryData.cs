using System;
using System.Collections.Generic;

[Serializable]
public class RunHistoryData
{
    public int schemaVersion = 1;

    public List<RunRecord> runs = new List<RunRecord>();
}