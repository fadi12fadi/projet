using System;
using System.Collections.Generic;

public class CollectionOfTheses
{
    private List<These> _approvedTheses;
    
    private List<These> _thesesRequested;

    public CollectionOfTheses()
    {
        _approvedTheses = new List<These>();
        _thesesRequested = new List<These>();
    }

    public void AddApprovedThese(These thesis)
    {
        _approvedTheses.Add(thesis);
    }

    public void AddTheseRequest(These thesis)
    {
        _thesesRequested.Add(thesis);
    }

    public bool RemoveApprovedThese(long id)
    {
        var thesis = _approvedTheses.Find(t => t.ID == id);
        return thesis != null && _approvedTheses.Remove(thesis);
    }

    public List<These> GetApprovedTheses() => new List<These>(_approvedTheses);

    public List<These> GetPendingThesesRequests() => new List<These>(_thesesRequested);

    public bool ApproveThese(long id)
    {
        var thesis = _thesesRequested.Find(t => t.ID == id);
        if (thesis != null)
        {
            _approvedTheses.Add(thesis);
            _thesesRequested.Remove(thesis);
            return true;
        }
        return false;
    }

    public bool RemovePendingThese(long id)
    {
        int removedCount = _thesesRequested.RemoveAll(t => t.ID == id);
        return removedCount > 0;
    }
}