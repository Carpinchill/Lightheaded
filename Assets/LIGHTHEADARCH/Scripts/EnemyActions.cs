using UnityEngine;  

public interface IEnemyActions
{
    //FRANCO
    void MoveToPlayer(Vector3 targetPosition); 
    void StartAttack(); 
    void StopAttack(); 
    void Patrol(); 
}
