using System.Collections;
using UnityEngine;

public interface IBattleFX
{
    public string Name { get; }
    public IEnumerator Inflict(Unit unit); 
}
