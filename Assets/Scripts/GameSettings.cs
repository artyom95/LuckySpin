using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(menuName = "Create GameSettings", fileName = "GameSettings", order = 0)]
    public class GameSettings: ScriptableObject

    {
        [field: SerializeField] public SpriteRenderer[] Sprites { get; private set; }
     
        [field:SerializeField] public Item ItemPrefab { get; private set; }
        
        [field:SerializeField] public int AmountAttempts { get; private set; }
        
        [field:SerializeField] public Vector3 LastMovingPosition { get; private set; }
        [field:SerializeField] public float JumpPower { get; private set; }
        [field:SerializeField] public int NumberJumps { get; private set; }
        [field:SerializeField] public float MoveDuration { get; private set; }
        [field:SerializeField] public float ScaleDuration { get; private set; }
        [field:SerializeField] public float ScaleAttemptDuration { get; private set; }

        [field:SerializeField] public float ScaleChestDuration { get; private set; }

        [field:SerializeField] public float RotateDuration { get; private set; }
        [field:SerializeField] public Vector3 RotateAngle { get; private set; }
        [field:SerializeField] public float ShakeDuration { get; private set; }

        [field:SerializeField] public float ChestХPosition { get; private set; }

        [field:SerializeField] public float MinimalSpeedRotation { get; private set; }
        [field:SerializeField] public float MaximalSpeedRotation { get; private set; }
        [field:SerializeField] public float MinimalAngleRotation { get; private set; }
        [field:SerializeField] public float MaximalAngleRotation { get; private set; }
        [field:SerializeField] public float Delay { get; private set; }

    }
}