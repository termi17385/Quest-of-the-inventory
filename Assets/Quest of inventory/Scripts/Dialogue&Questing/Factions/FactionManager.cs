using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Factions
{
    float approval;
    
    [ShowInInspector]
    public float Approval
    {
        set => approval = Mathf.Clamp(value, -1, 1);
        get => approval;
    }
}
public class FactionManager : SerializedMonoBehaviour
{
    [ShowInInspector] Dictionary<string, Factions> factions;
    public static FactionManager Instance;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
        
        factions = new Dictionary<string, Factions> {{"TheSettlementClan", 
            new Factions()}, {"Empire", new Factions()}};
    }

    /// <summary>
    /// Used for increasing/decreasing the approval for a faction
    /// </summary>
    /// <param name="_factionName">What faction we want to effect</param>
    /// <param name="_value">how much to increase/decrease approval by</param>
    /// <returns></returns>
    public float? FactionsApproval(string _factionName, float _value)
    {
        if (!factions.ContainsKey(_factionName)) return null;
        factions[_factionName].Approval += _value;
        return factions[_factionName].Approval;

    }

    public float? FactionsApproval(string _factionName)
    {
        if (factions.ContainsKey(_factionName))
        {
            return factions[_factionName].Approval;
        }

        return null;
    }
}
