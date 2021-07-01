using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    
    public List<EffectBase> ActiveEffects = new List<EffectBase>();

    public void AddEffect(EffectBase Effect){
        this.ActiveEffects.Add(Effect);
    }

    public void RemoveEffect(EffectBase Effect){
        this.ActiveEffects.Remove(Effect);
    }


}
