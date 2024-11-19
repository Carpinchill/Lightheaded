// IEnemyActions.cs
using UnityEngine;  // Aseg�rate de importar este espacio de nombres para acceder a Vector3

public interface IEnemyActions
{
    void MoveToPlayer(Vector3 targetPosition); // M�todo para mover al enemigo hacia una posici�n
    void StartAttack(); // M�todo para iniciar el ataque
    void StopAttack(); // M�todo para detener el ataque
    void Patrol(); // M�todo para patrullar
}
