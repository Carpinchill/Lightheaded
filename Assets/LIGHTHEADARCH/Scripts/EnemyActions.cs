// IEnemyActions.cs
using UnityEngine;  // Asegúrate de importar este espacio de nombres para acceder a Vector3

public interface IEnemyActions
{
    void MoveToPlayer(Vector3 targetPosition); // Método para mover al enemigo hacia una posición
    void StartAttack(); // Método para iniciar el ataque
    void StopAttack(); // Método para detener el ataque
    void Patrol(); // Método para patrullar
}
