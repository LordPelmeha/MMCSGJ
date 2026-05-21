# Руководство по настройке проекта "Half Empty"

## Содержание

1. [Требования](#1-требования)
2. [Создание проекта Unity](#2-создание-проекта-unity)
3. [Конфигурация проекта](#3-конфигурация-проекта)
4. [Установка пакетов](#4-установка-пакетов)
5. [Создание папок и структуры](#5-создание-папок-и-структуры)
6. [Layers и Tags](#6-layers-и-tags)
7. [ScriptableObject конфиги](#7-scriptableobject-конфиги)
8. [Настройка Input System](#8-настройка-input-system)
9. [Создание префабов](#9-создание-префабов)
10. [Настройка URP 2D](#10-настройка-urp-2d)
11. [Сборка сцены](#11-сборка-сцены)
12. [Проверка компиляции](#12-проверка-компиляции)
13. [Известные проблемы и решения](#13-известные-проблемы-и-решения)

---

## 1. Требования

| Параметр | Значение |
|---|---|
| Unity версия | 6000.3.14f1 (6) или 2019.4 LTS+ |
| Rendering Pipeline | Universal Render Pipeline (URP) 2D |
| .NET Standard | 4.x |
| Целевая платформа | PC, Windows (x64) |
| Целевое разрешение | 1920×1080, pixel-perfect |
| Pixel Per Unit | 16 |

---

## 2. Создание проекта Unity

1. Откройте Unity Hub → **New Project**
2. Выберите шаблон **2D URP** (Universal 2D)
3. Название проекта: `Half Empty`
4. Место расположения: `E:\MMCSGJ\` (папка с корнем проекта)
5. Создайте проект и дождитесь открытия редактора

Важно: при использовании Unity 6.x установите в **Project Settings → Player → Other Settings → Configuration → Api Compatibility Level** значение `.NET Framework` или `.NET Standard 2.1`.

Для Unity 6000.x оставьте по умолчанию — используется `#nullable enable` и C# 10 features.

---

## 3. Конфигурация проекта

### 3.1 `Directory.Build.props`

Создайте файл `Directory.Build.props` в корне проекта для установки версии языка C#:

```xml
<Project>
  <PropertyGroup>
    <LangVersion>latest</LangVersion>
  </PropertyGroup>
</Project>
```

### 3.2 Множитель масштаба (Pixel Perfect)

1. Откройте окно **Project Settings → Player → Resolution and Presentation**
2. Установите **Default Canvas Scaler → UI Scale Mode** = `Scale With Screen Size`
3. **Reference Resolution** = `1920 x 1080`
4. **Match** = `0.5` (сбалансированное масштабирование по ширине и высоте)

### 3.3 Physics 2D Settings

1. Откройте **Project Settings → Physics 2D**
2. **Gravity Y** = `-9.81` (стандарт)
3. **Velocity Iterations** = `8`
4. **Position Iterations** = `3`
5. **Queries Hit Triggers** = ✅ включено

---

## 4. Установка пакетов

### 4.1 Обязательные пакеты

| Пакет | Версия | Назначение |
|---|---|---|
| com.unity.inputsystem | 1.7.0+ | Новая система ввода вместо старого `Input.GetAxis` |
| com.unity.2d.sprite | 1.0.0+ | Работа со спрайтами 2D |
| com.unity.2d.tilemap | 1.0.0+ | Тайлмапы уровней |
| com.unity.2d.animation | 9.0.0+ | Анимации 2D (Spine не требуется) |
| com.unity.shadergraph | 14.0.0+ (URP) | Визуальный редактор шейдеров для Fog of War |

Для установки откройте **Window → Package Manager**, нажмите **+ → Add package by name** и введите имя пакета.

### 4.2 Рекомендуемые инструменты

| Пакет | Назначение |
|---|---|
| com.unity.ide.rider | Интеграция с IDE |
| com.unity.nuget.newtonsoft-json | JSON-сериализация при необходимости |

---

## 5. Создание папок и структуры

### 5.1 Базовая структура

Создайте следующую структуру папок в `Assets/`:

```
Assets/
├── _Project/
│   ├── Application/
│   │   ├── Enemies/
│   │   │   └── States/
│   │   ├── Game/
│   │   │   └── GameStates/
│   │   ├── Player/
│   │   │   ├── States/
│   │   │   └── ...
│   │   ├── StateMachine/
│   │   └── ...
│   ├── Domain/
│   │   ├── Combat/
│   │   ├── Enums/
│   │   └── Health/
│   ├── Infrastructure/
│   │   ├── Configs/
│   │   ├── Events/
│   │   ├──Factories/
│   │   ├── Input/
│   │   └── Pools/
│   ├── Presentation/
│   │   ├── Camera/
│   │   ├── Combat/
│   │   ├── Enemies/
│   │   ├── Game/
│   │   ├── Player/
│   │   ├── UI/
│   │   └── Vision/
│   ├── Art/
│   │   ├── Animations/
│   │   ├── Sprites/
│   │   │   ├── Player/
│   │   │   ├── Enemies/
│   │   │   ├── Projectiles/
│   │   │   ├── Environment/
│   │   │   ├── UI/
│   │   │   └── VFX/
│   │   └── Tilemaps/
│   │       ├── Palettes/
│   │       └── Tiles/
│   ├── Audio/
│   │   ├── SFX/
│   │   └── Music/
│   ├── Configs/
│   │   ├── Player/
│   │   ├── Enemies/
│   │   ├── Projectiles/
│   │   ├── Camera/
│   │   └── Vision/
│   ├── Prefabs/
│   │   ├── Player/
│   │   ├── Enemies/
│   │   ├── Projectiles/
│   │   ├── UI/
│   │   └── Environment/
│   ├── Scenes/
│   └── editor/
├── StreamingAssets/
└── Plugins/
```

> **Примечание:** папка `_Project` (с подчёркиванием) отделяет игровой код от стандартных папок Unity (`Editor`, `Plugins` и т.д.), чтобы не было конфликтов имён.

### 5.2 Правила именования файлов

- **Скрипты**: `PascalCase.cs` — например, `PlayerController.cs`, `HealthData.cs`
- **ScriptableObject инстансы**: `PascalCase.asset` — например, `PlayerConfig.asset`
- **Папки**: `PascalCase` — например, `Presentation`, `Infrastructure`
- **Конфиги**: `{Имя}Config.cs` для класса, `{Имя}Config.asset` для инстанса

---

## 6. Layers и Tags

### 6.1 Создание Layers

Откройте **Edit → Project Settings → Tags and Layers → Layers** и добавьте:

```
Player
Enemy
PlayerProjectile
EnemyProjectile
Environment
Trap
Interactable
```

### 6.2 Создание Tags

В том же окне на вкладке **Tags** добавьте:

```
Player
Enemy
Projectile
MeleeAttack
ParryHitbox
Trap
```

### 6.3 Матрица столкновений (Collision Matrix)

| | Player | Enemy | PlayerProj | EnemyProj | Environment | Trap |
|---|---|---|---|---|---|---|
| **Player** | — | ✅ | — | ✅ | ✅ | ✅ |
| **Enemy** | ✅ | — | ✅ | — | ✅ | — |
| **PlayerProj** | — | ✅ | — | — | ✅ | — |
| **EnemyProj** | ✅ | — | — | — | ✅ | — |
| **ParryHitbox** | — | ✅ | — | ✅ | — | — |

Настройка матрицы доступна в **Edit → Project Settings → Physics 2D → Layer Collision Matrix**.

Автоматическую настройку выполняет скрипт `Assets/Editor/PhysicsLayerCollisionSetup.cs`:

```csharp
[InitializeOnLoad]
public static class PhysicsLayerCollisionSetup
{
    [RuntimeInitializeOnLoadMethod]
    static void Configure() { /* ... */ }
}
```

При запуске редактора он устанавливает все нужные галочки за один клик.

---

## 7. ScriptableObject конфиги

### 7.1 Общий принцип

Все числовые параметры баланса вынесены в отдельные ScriptableObject-ассеты. Это позволяет:
- Балансировать игру без открытия кода
- Создавать несколько вариантов конфигов для тестирования
- Не терять параметры при рефакторинге скриптов

### 7.2 Список необходимых ассетов

| Ассет | Путь | Класс |
|---|---|---|
| `PlayerConfig.asset` | `Assets/Configs/Player/` | `PlayerConfigSO` |
| `HeadFormConfig.asset` | `Assets/Configs/Player/` | `FormConfigSO` |
| `BodyFormConfig.asset` | `Assets/Configs/Player/` | `FormConfigSO` |
| `MeleeEnemyConfig.asset` | `Assets/Configs/Enemies/` | `EnemyConfigSO` |
| `RangedEnemyConfig.asset` | `Assets/Configs/Enemies/` | `EnemyConfigSO` |
| `PlayerProjectileConfig.asset` | `Assets/Configs/Projectiles/` | `ProjectileConfigSO` |
| `EnemyProjectileConfig.asset` | `Assets/Configs/Projectiles/` | `ProjectileConfigSO` |
| `CameraConfig.asset` | `Assets/Configs/Camera/` | `CameraConfigSO` |
| `VisionConfig.asset` | `Assets/Configs/Vision/` | `VisionConfigSO` |
| `OnPlayerDeath.asset` | `Assets/Configs/Events/` | `VoidEventSO` |
| `OnFormSwitched.asset` | `Assets/Configs/Events/` | `VoidEventSO` |
| `OnParrySuccess.asset` | `Assets/Configs/Events/` | `VoidEventSO` |

### 7.3 Значения по умолчанию

```
PlayerConfigSO:
  formSwitchCooldown = 0.5f
  parryWindow = 0.3f
  parryCooldown = 0.5f
  headFormConfig → ссылка на HeadFormConfig.asset
  bodyFormConfig → ссылка на BodyFormConfig.asset

FormConfigSO (для головы):
  maxHP = 100
  moveSpeed = 2.0f
  canJump = false
  canDash = false
  shootDamage = 25f
  shootRate = 0.4f
  projectileSpeed = 15f
  damageMultiplier = 1.0f
  markDuration = 10f
  maxMarks = 5
  fullVision = true
  innerVisionRadius = 3f
  outerVisionRadius = 5f

FormConfigSO (для тела):
  maxHP = 75
  moveSpeed = 6.0f
  canJump = true
  jumpForce = 12f
  canDash = true
  dashDistance = 4f
  dashDuration = 0.15f
  dashCooldown = 1.0f
  dashInvincible = true
  shootDamage = 12f
  shootRate = 0.3f
  projectileSpeed = 12f
  damageMultiplier = 0.5f
  fullVision = false
  innerVisionRadius = 3f
  outerVisionRadius = 5f
```

### 7.4 Создание ассетов через редактор

Каждый ScriptableObject имеет атрибут `[CreateAssetMenu]`, поэтому ассеты создаются через контекстное меню:

1. В окне **Project** кликните правой кнопкой на папке (например, `Configs/Player/`)
2. **Create → Configs → Player Config** (или соответствующий тип)
3. Задайте значения в инспекторе
4. Сохраните ассет

---

## 8. Настройка Input System

### 8.1 Включение пакета

Если Input System не был включен при создании проекта:

1. **Window → Package Manager**
2. Найдите **Input System** → **Install**

### 8.2 Создание Input Actions

Создайте файл `Assets/Infrastructure/Input/InputSystem_Actions.inputactions`:

1. В окне **Project** кликните правой кнопкой → **Create → Input Actions**
2. Назовите файл `InputSystem_Actions`
3. В инспекторе нажмите **Edit Asset** — откроется окно **Input Actions**
4. Создайте Action Map с именем `Player`
5. Добавьте действия:

| Action Name | Type | Binding |
|---|---|---|
| `Move` | Value → Vector2 | `A` / `D` (`<Keyboard>/a`, `<Keyboard>/d`) |
| `Look` | Value → Vector2 | `<Mouse>/delta` |
| `Shoot` | Button | `<Mouse>/leftButton` |
| `Parry` | Button | `<Mouse>/rightButton` |
| `Mark` | Button | `<Keyboard>/m` |
| `SwitchForm` | Button | `<Keyboard>/leftShift` |
| `Pause` | Button | `<Keyboard>/escape` |
| `Jump` | Button | `<Keyboard>/space` |
| `Dash` | Button | `<Keyboard>/leftCtrl` |

6. Сохраните — Unity автоматически сгенерирует класс `InputSystem_Actions` в корне проекта.

### 8.3 Назначение на объект

Перетащите созданный ассет `InputSystem_Actions` в поле `_inputActions` компонента `UnityInputProvider` на префабе игрока.

---

## 9. Создание префабов

### 9.1 Префаб игрока

1. Создайте пустой GameObject в сцене, назовите `Player`
2. Добавьте компоненты:
   - `PlayerController` (основной координатор)
   - `Rigidbody2D`: **Body Type** = `Dynamic`, **Gravity Scale** = `3`, **Constraints** → `Freeze Rotation Z`
   - `Collider2D` (например, `CapsuleCollider2D`): настройте размер под спрайт
   - `UnityInputProvider` (если MonoBehaviour используется)
   - Дочерние GameObject:
     - `HeadPart` — спрайт верхней половины (голова/руки)
     - `BodyPart` — спрайт нижней половины (торс/ноги)
3. Назначьте ссылки в инспекторе `PlayerController`:
   - `_rb` → `Rigidbody2D`
   - `_mainCollider` → `Collider2D`
   - `_headPart` / `_bodyPart` → соответствующие трансформы
   - Все суб-вью (`_movementView`, `_combatView`, `_healthView`, `_visionView`, `_animationView`, `_markView`)
   - `_parryHitbox` → объект с `ParryHitboxView`
   - `_config` → `PlayerConfig.asset`
4. Сохраните в `Assets/Prefabs/Player/Player.prefab`

### 9.2 Префаб снаряда

1. Пустой GameObject с `ProjectileView` + `Rigidbody2D` (kinematic) + `Collider2D` (trigger) + `SpriteRenderer`
2. Сохраните в `Assets/Prefabs/Projectiles/PlayerProjectile.prefab`

### 9.3 Префаб врага

1. GameObject с `MeleeEnemyView` / `RangedEnemyView` + `Rigidbody2D` + `Collider2D` + `SpriteRenderer`
2. Сохраните в `Assets/Prefabs/Enemies/MeleeEnemy.prefab` и `RangedEnemy.prefab`

### 9.4 Префаб ParryHitbox

1. Пустой GameObject как дочерний к игроку, впереди него
2. Компонент `ParryHitboxView` + `Collider2D` (trigger)
3. Сохраните и назначьте на `PlayerController._parryHitbox`

---

## 10. Настройка URP 2D

### 10.1 Asset → Create → Render Pipeline → Universal Render Pipeline → Pipeline Asset (Forward Renderer)

Если проект был создан по шаблону 2D URP — этот шаг уже выполнен.

### 10.2 Настройка камеры

1. Выберите **Main Camera**
2. **Projection** = `Orthographic`
3. **Size** = `8.0` (половина высоты видимой области в единицах мира; соответствует `VisionConfig.innerVisionRadius`)
4. **Clear Flags** = `Solid Color`
5. **Background** = чёрный цвет `#000000`

### 10.3 Слои сортировки спрайтов (Sorting Layers)

Создайте в **Project Settings → Tags and Layers → Sorting Layers**:

```
Default      (order: 0)
Background   (order: 1)
Environment  (order: 5)
Enemy        (order: 10)
Player       (order: 10)
Projectile   (order: 15)
UI           (order: 20)
```

---

## 11. Сборка сцены

### 11.1 Создание основной сцены

1. **File → New Scene → 2D (URP)** → сохраните как `Assets/Scenes/Level_01.unity`
2. В корне сцены добавьте:
   - **GameManager** — пустой GameObject, компонент `GameManager`, подпись на `_onPlayerDeath` → `OnPlayerDeath` Event, при вызове — переход на `Game Over` сцену
   - **GameFlowSM** — пустой GameObject, компонент `GameFlowSM`
   - **Main Camera** — с настроенной `CameraController`
   - **Parallax background** — паттерн-спрайт заднего плана для эффекта глубины

### 11.2 Размещение объектов уровня

1. **Tilemaps**: нарисуйте платформы, пол, стены
2. **Player Spawn** — точка появления игрока (с тегом `Player`)
3. **Enemy Spawn Points** — empty GameObjects с тегом `Enemy`
4. **Trap points** — шипы с тегом `Trap` и слоем `Trap`

### 11.3 Сцена главного меню

Сохраните как `Assets/Scenes/MainMenu.unity`:
- `MainMenuView` — кнопки Play / Quit
- `GameManager` — переход на `Level_01` при нажатии Play

### 11.4 Иерархия сцены (пример)

```
Level_01
├── BackgroundLayer
│   └── Parallax_Background
├── EnvironmentLayer
│   ├── Platforms (Tilemap)
│   ├── Walls (Tilemap)
│   └── Traps
├── EnemyLayer
│   ├── Enemy_Spawn_01 → префаб MeleeEnemy
│   └── Enemy_Spawn_02 → префаб RangedEnemy
├── Player
│   ├── HeadPart
│   └── BodyPart
├── Projectiles (пустой, для спавна снарядов)
├── UILayer
│   ├── HUDCanvas
│   └── PauseMenu
├── Main Camera [CameraController]
├── GameManager
└── GameFlowSM
```

---

## 12. Проверка компиляции

### 12.1 Проверка баланса скобок

Все C# файлы должны сбалансировать открывающие и закрывающие фигурные скобки. Проверьте скриптом:

```powershell
# check_balance.ps1
Get-ChildItem -Recurse -Filter "*.cs" | ForEach-Object {
    $content = Get-Content $_.FullName -Raw
    $opens = ($content.ToCharArray() | Where-Object { $_ -eq '{' }).Count
    $closes = ($content.ToCharArray() | Where-Object { $_ -eq '}' }).Count
    if ($opens -ne $closes) {
        Write-Host "IMBALANCE: $($_.Name) open=$opens close=$closes"
    }
}
```

### 12.2 Принудительная перекомпиляция

Если Unity не компилирует после изменений, выполните в редакторе:

```
Assets → Recompile All Scripts
```

Или через инструменты разработчика:
- Unity 6.x: **Assets → Recompile All Scripts**
- Unity 2019–2022: зайдите в **Edit → Preferences → External Tools → Regenerate project files**

### 12.3 Типичные ошибки и их исправление

| Ошибка | Причина | Исправление |
|---|---|---|
| `CS0246: type not found` | Отсутствует `using`-директива | Добавьте нужный `using {namespace}` |
| `CS1513: } expected` | Лишняя/недостающая `}` или сломанная структура класса | Проверьте баланс скобок |
| `CS5240: CS1061: not found` | Метод/свойство отсутствует в классе | Проверьте наличие метода в интерфейсе/классе |
| `CS1014: A get or set accessor expected` | Инструкция снаружи блока get/set в св-ве | Перенесите тело свойства внутрь блоков `get `/ `set ` |
| `CS0161: not all code paths return` | Getter/метод без return в ветке else | Добавьте значение по умолчанию перед закрытием `}` |
| `CS0103: does not exist` | Имя метода не определено | Добавьте missing метод или исправьте опечатку |
| `CS8618: non-nullable field` | Поле без инициализации в конструкторе | Добавьте `?` к типу или инициализируйте значение |

---

## 13. Известные проблемы и решения

### 13.1 Unity перезаписывает редактируемые файлы

Иногда In-Process компилятор Unity может перезаписать изменения в файлах C# сразу после сохранения. Это происходит когда:

- Файл открыт одновременно редактором и в скриптовом редакторе
-Unity перекомпилирует и перезагружает домен приложения

**Решение**: после внесения изменений в C# файл всегда сделайте **Assets → Recompile All Scripts** и проверьте консоль. `ForceRecompileAll.sh` не включен в репозиторий — используйте стандартную перезагрузку Unity.

### 13.2 Дублирование namespace в `HalfEmpty.Presentation.Enemies`

Убедитесь, что namespace у файлов совпадает с физическим расположением в папках:

- `EnemyView.cs` → `namespace HalfEmpty.Presentation` (НЕ `.Presentation.Enemies`)
- `MeleeEnemyView.cs` → `namespace HalfEmpty.Presentation.Enemies`
- State files в `Application/Enemies/States/` используют `using HalfEmpty.Presentation` для доступа к `EnemyView`

### 13.3 Отсутствующие классы (MarkManager и т.д.)

Некоторые классы являются заглушками и не имеют полной реализации. При необходимости реализовать sought в отдельном файле совпадающем по namespace.

### 13.4 Input System Actions.cs — стаб

В проекте присутствует `Infrastructure/Input/InputSystem_Actions.cs` — это временный стаб без полной генерации Unity. После создания `.inputactions` файла в проекте Unity перегенерирует этот класс автоматически. Стаб удалите, чтобы не было конфликтов имён.

### 13.5 End-of-line (EOL) конфликты

Все `.cs` файлы в проекте используют `\r\n` (CRLF) для совместимости с Unity 6.x Burst Compiler. Не пересохраняйте файлы с EOL = `\n` (LF), это вызывает `CS1513: } expected` из-за некорректного парсинга промежуточного контента.

---

## Быстрый чек-лист перед запуском

- [ ] Unity 6.x открыта, проект загружен без ошибок компиляции
- [ ] `Directory.Build.props` в корне проекта
- [ ] Все ScriptableObject ассеты созданы в папках `Assets/Configs/`
- [ ] Input System Actions создан и сгенерирован
- [ ] Layers и Tags добавлены в проект
- [ ] Physics 2D Collision Matrix настроен (или скрипт `PhysicsLayerCollisionSetup.cs` запущен)
- [ ] Сцена `Level_01.unity` создана в `Assets/Scenes/`
- [ ] Префабы Player/Enemies/Projectiles созданы в `Assets/Prefabs/`
- [ ] Консоль Unity не показывает ошибок CS0XXX/CS1XXX/CS2XXX
- [ ] Уровень добавлен в **File → Build Settings → Scenes In Build**
