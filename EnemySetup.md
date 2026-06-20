# Enemy Setup Checklist

This checklist covers the specific setup needed for the enemy functionality that was implemented.

## Prerequisites (Already Done)
- [x] EnemyController.cs created with state machine
- [x] EnemyStateMachine.cs created for state transitions
- [x] EnemyConfigSO updated with EnemyType, playerLayerMask, projectileConfig
- [x] ProjectilePool has Factory property for projectile creation
- [x] Enemy prefabs have EnemyController component added
- [x] EnemyProjectile.prefab created
- [x] Projectile configs have correct targetLayer values
- [x] ParryHitboxView has OnTriggerEnter2D for parry detection
- [x] VoidEventSO and FloatEventSO have Raise() methods
- [x] EnemyShootState uses fire point from RangedEnemyView

## What You Need To Do

### 1. Initialize Enemies in Scene
Enemies require manual initialization. Add this to your scene setup:

```csharp
// In a spawner or GameInitializer
public void SpawnMeleeEnemy(Vector3 position, EnemyConfigSO config) {
    var prefab = Instantiate(meleeEnemyPrefab, position, Quaternion.identity);
    var controller = prefab.GetComponent<EnemyController>();
    var player = FindObjectOfType<PlayerController>().transform;
    var view = prefab.GetComponent<EnemyView>();
    view.Initialize(config, FormType.Body);
    // If EnemyController has _playerTarget serialized field, assign it
    // Otherwise, modify EnemyController to find PlayerController.transform
}
```

### 2. Assign Player Target to EnemyController
The EnemyController needs a reference to the player transform:
- Select enemy prefab in scene
- In Inspector, find `EnemyController` component
- Drag the Player GameObject to the `_playerTarget` field

### 3. Verify Projectile Pool Setup
Ensure in Main.unity:
- ProjectilePool has PlayerProjectile prefab assigned to `_prefab`
- The pool object is active in the scene

### 4. Test Enemy Behavior
Play the scene to verify:
- [ ] Melee enemy patrols (Idle state)
- [ ] Melee enemy chases when player enters detection range
- [ ] Melee enemy attacks when in range
- [ ] Ranged enemy tracks player
- [ ] Ranged enemy shoots projectiles at player
- [ ] Enemy death triggers death animation and destroys after delay

### 5. (Optional) Enemy Spawner
Create a spawner component to place enemies:
```csharp
public class EnemySpawner : MonoBehaviour {
    [SerializeField] private EnemyConfigSO _meleeConfig;
    [SerializeField] private EnemyConfigSO _rangedConfig;
    [SerializeField] private GameObject _meleePrefab;
    [SerializeField] private GameObject _rangedPrefab;
    
    private void Start() {
        SpawnEnemies();
    }
    
    private void SpawnEnemies() {
        // Initialize each spawned enemy with config
        foreach (Transform spawnPoint in transform) {
            var prefab = spawnPoint.name.Contains("Ranged") ? _rangedPrefab : _meleePrefab;
            var config = spawnPoint.name.Contains("Ranged") ? _rangedConfig : _meleeConfig;
            var enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
            var view = enemy.GetComponent<EnemyView>();
            view.Initialize(config, FormType.Body);
        }
    }
}
```

### 6. Verify Layer Collision Matrix
Check `Project Settings → Physics 2D → Layer Collision Matrix`:
| | Player | Enemy | PlayerProjectile | EnemyProjectile |
|---|---|---|---|---|
| **Player** | — | ✅ | — | ✅ |
| **Enemy** | ✅ | — | ✅ | — |
| **PlayerProjectile** | — | ✅ | — | — |
| **EnemyProjectile** | ✅ | — | — | — |

## Troubleshooting

**Enemy doesn't move:**
- Check that EnemyController has `_playerTarget` assigned
- Check `EnemyConfigSO.moveSpeed > 0`

**Enemy doesn't attack:**
- Check `EnemyConfigSO.attackRange` is reasonable (e.g., 1.5)
- Check that player is within attack range in chase state

**Projectiles don't spawn from ranged enemy:**
- Check `RangedEnemyConfig.projectileConfig` is assigned
- Check ProjectilePool is in scene with prefab assigned