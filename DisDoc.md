

# Game Design Document: "Half Empty"

## Оглавление

1. [Концепция](#1-концепция)
2. [Игровые механики](#2-игровые-механики)
3. [Игрок](#3-игрок)
4. [Враги](#4-враги)
5. [Система боя](#5-система-боя)
6. [Камера и визуальное представление](#6-камера-и-визуальное-представление)
7. [Уровни и окружение](#7-уровни-и-окружение)
8. [UI/UX](#8-uiux)
9. [Архитектура проекта](#9-архитектура-проекта)
10. [Структура папок](#10-структура-папок)
11. [Описание основных классов](#11-описание-основных-классов)
12. [ScriptableObject конфиги](#12-scriptableobject-конфиги)
13. [Roadmap разработки](#13-roadmap-разработки)

---

## 1. Концепция

### 1.1 Название
**"Half Empty"**

### 1.2 Жанр
2D пиксельный платформер-шутер с элементами тактики (аналог — My Friend Pedro)

### 1.3 Тема джема
**«Наполовину пуст»** — буквальная интерпретация: персонаж разделён на две половины (голова/руки и тело/ноги). Каждая половина — это отдельный режим со своими способностями и ограничениями. Ни одна из форм не является полноценной сама по себе — игрок всегда «наполовину пуст».

### 1.4 Elevator Pitch
Ты — существо, разделённое надвое. В режиме «Головы» ты видишь всё поле боя, метишь врагов и стреляешь точно, но едва ползёшь. В режиме «Тела» ты быстр и ловок, но почти слеп. Переключайся между половинами, чтобы выжить.

### 1.5 Целевая платформа
PC (Windows)

### 1.6 Управление
Клавиатура + мышь

---

## 2. Игровые механики

### 2.1 Переключение форм
| Параметр | Описание |
|---|---|
| **Клавиша** | `Shift` (настраивается) |
| **Время переключения** | Мгновенно (0 кадров задержки геймплея, визуальная анимация ~0.2 сек) |
| **Кулдаун** | Настраиваемый, по умолчанию — 0.5 сек |
| **Позиция** | Персонаж остаётся на том же месте |
| **Здоровье** | У каждой формы своя шкала HP; при переключении HP другой формы не восстанавливается |

### 2.2 Передвижение

| Действие | Клавиша | Форма «Голова» | Форма «Тело» |
|---|---|---|---|
| Движение влево | `A` | Медленно (настр.) | Быстро (настр.) |
| Движение вправо | `D` | Медленно (настр.) | Быстро (настр.) |
| Прыжок | `Space` | **Недоступен** | Доступен |
| Рывок (Dash) | `LCtrl` | **Недоступен** | Доступен, в направлении движения |

### 2.3 Стрельба

| Параметр | Форма «Голова» | Форма «Тело» |
|---|---|---|
| Клавиша | `ЛКМ` | `ЛКМ` |
| Направление | В сторону курсора мыши | В сторону курсора мыши |
| Урон | Полный (настр.) | Сниженный (настр., по умолч. ×0.5) |
| Скорость снаряда | Настраиваемая | Настраиваемая (может отличаться) |
| Скорострельность | Настраиваемая | Настраиваемая |

### 2.4 Метки (Marking)

| Параметр | Описание |
|---|---|
| **Доступность** | Только в форме «Голова» |
| **Клавиша** | `ПКМ` |
| **Действие** | Игрок наводит курсор на врага/объект/ловушку и нажимает ПКМ — объект обводится красным контуром |
| **Видимость метки** | Метка остаётся видимой **в обеих формах**, включая форму «Тело» (даже за пределами ограниченного обзора — метка "просвечивает" через туман) |
| **Время жизни метки** | Настраиваемое, по умолчанию — 10 сек |
| **Макс. кол-во меток** | Настраиваемое, по умолчанию — 5 |

### 2.5 Парирование

| Параметр | Описание |
|---|---|
| **Клавиша** | `ПКМ` (в форме «Тело»), `F` (альтернатива, настраиваемая) |
| **Окно парирования** | Настраиваемое, по умолчанию 0.3 сек |
| **Эффект при успешном парировании** | Атака врага отражается обратно в него; враг получает летальный урон (мгновенная смерть) |
| **Что можно парировать** | Ближнюю атаку мили-врага; снаряд стрелка |
| **Парирование снаряда** | Снаряд разворачивается и летит обратно к стрелку с увеличенной скоростью |
| **Парирование мили-атаки** | Враг получает мгновенный летальный урон, воспроизводится эффект контратаки |
| **Кулдаун** | Настраиваемый, по умолчанию 0.5 сек |
| **Доступность** | Обе формы |

### 2.6 Рывок (Dash)

| Параметр | Описание |
|---|---|
| **Доступность** | Только форма «Тело» |
| **Направление** | В сторону текущего движения (влево/вправо). Если игрок стоит — в сторону, куда смотрит |
| **Дистанция** | Настраиваемая |
| **Длительность** | Настраиваемая (~0.15 сек) |
| **I-frames** | Во время рывка игрок неуязвим |
| **Кулдаун** | Настраиваемый, по умолчанию 1 сек |

---

## 3. Игрок

### 3.1 Общие параметры

```
PlayerConfig (ScriptableObject):
├── HeadFormConfig
│   ├── maxHP: int = 100
│   ├── moveSpeed: float = 2.0
│   ├── canJump: bool = false
│   ├── canDash: bool = false
│   ├── shootDamage: float = 25
│   ├── shootRate: float = 0.4 (сек между выстрелами)
│   ├── projectileSpeed: float = 15
│   ├── markDuration: float = 10
│   ├── maxMarks: int = 5
│   └── damageMultiplier: float = 1.0
│
├── BodyFormConfig
│   ├── maxHP: int = 75
│   ├── moveSpeed: float = 6.0
│   ├── canJump: bool = true
│   ├── jumpForce: float = 12
│   ├── canDash: bool = true
│   ├── dashDistance: float = 4.0
│   ├── dashDuration: float = 0.15
│   ├── dashCooldown: float = 1.0
│   ├── dashInvincible: bool = true
│   ├── shootDamage: float = 12
│   ├── shootRate: float = 0.3
│   ├── projectileSpeed: float = 12
│   └── damageMultiplier: float = 0.5
│
├── formSwitchCooldown: float = 0.5
├── parryWindow: float = 0.3
└── parryCooldown: float = 0.5
```

### 3.2 Здоровье

- Каждая форма имеет **независимую шкалу HP**.
- При смерти одной формы — **Game Over** (персонаж не может существовать без одной из половин).
- Урон получает **только активная форма**.
- HP не регенерируется автоматически (возможны аптечки-пикапы на уровнях).

### 3.3 Визуальное представление

- **Форма «Голова»**: спрайт верхней половины тела (голова, руки, оружие). Парит/ползёт по земле. Анимации: idle, move, shoot, mark, switch, hurt, death.
- **Форма «Тело»**: спрайт нижней половины тела (торс, ноги). Бежит, прыгает. Анимации: idle, run, jump, fall, dash, shoot, parry, hurt, death.

---

## 4. Враги

### 4.1 Тип 1: Бегун (Melee Runner)

| Параметр | Значение (по умолчанию) |
|---|---|
| HP | 50 |
| Скорость передвижения | 4.0 |
| Урон атаки | 20 |
| Дистанция атаки | 1.5 (юниты) |
| Кулдаун атаки | 1.0 сек |
| Дальность обнаружения | 10 юнитов |

**Поведение:**
1. **Idle** — стоит на месте / патрулирует между двумя точками (настраиваемо).
2. **Chase** — обнаруживает игрока (raycast / триггер-зона) → бежит к нему.
3. **Attack** — дистанция до игрока ≤ attackRange → совершает ближнюю атаку.
4. **Death** — HP ≤ 0 → анимация смерти, уничтожение объекта.

**Настраиваемые параметры (Inspector / ScriptableObject):**
```
MeleeEnemyConfig:
├── hp: int
├── moveSpeed: float
├── attackDamage: float
├── attackRange: float
├── attackCooldown: float
├── detectionRange: float
├── patrolPoints: Transform[] (опционально)
└── canBeParried: bool = true
```

### 4.2 Тип 2: Стрелок (Ranged Turret)

| Параметр | Значение (по умолчанию) |
|---|---|
| HP | 30 |
| Скорость передвижения | 0 (стоит на месте) |
| Урон снаряда | 15 |
| Скорость снаряда | 10 |
| Скорострельность | 1 выстрел / 1.5 сек |
| Угол обзора | 180° (перед собой) |
| Дальность обзора | 12 юнитов |

**Поведение:**
1. **Idle** — стоит на месте, сканирует зону перед собой.
2. **Shoot** — игрок попадает в поле видимости → стреляет с заданной скорострельностью.
3. **Death** — HP ≤ 0 → анимация смерти.

**Настраиваемые параметры:**
```
RangedEnemyConfig:
├── hp: int
├── attackDamage: float
├── projectileSpeed: float
├── fireRate: float
├── detectionAngle: float
├── detectionRange: float
├── canBeParried: bool = true
└── projectilePrefab: GameObject
```

### 4.3 Общее поведение врагов

- Враги, помеченные меткой (Mark), обводятся красным контуром — видимы через туман формы «Тело».
- При парировании вражеской атаки — враг **умирает мгновенно**.
- Враги не различают формы игрока — атакуют любую.

---

## 5. Система боя

### 5.1 Снаряды (Projectiles)

```
ProjectileConfig:
├── damage: float
├── speed: float
├── lifetime: float = 5.0 сек
├── size: Vector2 (коллайдер)
├── sprite: Sprite
├── isReflected: bool = false (устанавливается при парировании)
├── reflectedSpeedMultiplier: float = 1.5
└── layerMask: LayerMask (что может поразить)
```

**Логика:**
- Снаряд двигается в указанном направлении с заданной скоростью.
- При столкновении с целевым слоем — наносит урон и уничтожается.
- При столкновении с окружением — уничтожается.
- При парировании — `isReflected = true`, разворачивается, получает `reflectedSpeedMultiplier`, меняет целевой слой на «Enemy».

### 5.2 Парирование — подробный flow

```
1. Игрок нажимает клавишу парирования
2. Проверяется кулдаун → если не прошёл — игнорировать
3. Активируется parry-хитбокс (trigger collider) перед игроком на parryWindow секунд
4. Запускается анимация парирования
5. Если в parry-хитбокс попадает:
   a. Вражеский снаряд → снаряд отражается (разворот + смена layerMask)
   b. Мили-атака врага (определяется через тег/компонент) → враг получает летальный урон
6. По окончании parryWindow — хитбокс деактивируется
7. Запускается parryCooldown
```

### 5.3 Урон и смерть

```
Расчёт урона:
  finalDamage = baseDamage * targetVulnerabilityMultiplier (по умолчанию 1.0)

Смерть игрока:
  Если HP любой из форм ≤ 0 → Game Over → показ экрана перезапуска

Смерть врага:
  HP ≤ 0 → анимация смерти → деактивация коллайдеров → через N секунд Destroy
```

---

## 6. Камера и визуальное представление

### 6.1 Форма «Голова» — полный обзор

- Камера работает стандартно: следует за игроком, показывает окружение в полном объёме.
- Вся геометрия уровня, враги, ловушки — видимы.
- Камера может немного смещаться в сторону курсора мыши (параллакс-подобный эффект) для лучшего прицеливания.

**Параметры камеры (настраиваемые):**
```
CameraConfig:
├── headForm_followSmoothing: float = 5.0
├── headForm_cursorInfluence: float = 2.0 (смещение к курсору)
├── headForm_orthoSize: float = 8.0
```

### 6.2 Форма «Тело» — ограниченный обзор

Реализация через **маску видимости** (Shader/SpriteMask/RenderTexture):

- **Внутренний круг (clear zone):**
  - Радиус: настраиваемый (по умолчанию 3 юнита)
  - Всё видно чётко, полная яркость и непрозрачность
  - Враги, объекты, тайлы — полностью видимы

- **Внешний круг (dim zone):**
  - Радиус: настраиваемый (по умолчанию 5 юнитов)
  - Всё затемнено (alpha ~0.3–0.5, настраиваемо)
  - Объекты различимы, но тусклые

- **За пределами внешнего круга:**
  - Полная темнота (чёрный / не рендерится)
  - **Исключение:** помеченные объекты (Mark) видны как красные силуэты/контуры

**Техническая реализация:**
- Используется полноэкранный overlay-спрайт чёрного цвета с вырезанными кругами (shader с radial gradient)
- Альтернатива: Unity 2D Light system (URP) — point light на игроке с двумя радиусами

```
VisionConfig:
├── innerRadius: float = 3.0
├── outerRadius: float = 5.0
├── outerAlpha: float = 0.4
├── darknessAlpha: float = 1.0
├── markVisibilityThroughDarkness: bool = true
└── transitionSmoothness: float = 0.5
```

---

## 7. Уровни и окружение

### 7.1 Структура уровня

- Платформы (тайлмап / отдельные коллайдеры)
- Зоны спавна врагов (триггер-зоны / предустановленные позиции)
- Ловушки (шипы, движущиеся платформы — помечаемые через метки)
- Точка старта игрока
- Точка финиша / триггер перехода

### 7.2 Ловушки

| Ловушка | Описание | Можно пометить |
|---|---|---|
| Шипы | Статичные, наносят урон при контакте | Да |
| Движущаяся платформа | Перемещается между точками | Нет (не опасна) |

### 7.3 Scope для джема

Для 4-дневного джема планируется:
- **1 обучающий уровень** (знакомство с механиками)
- **2–3 боевых уровня** с нарастающей сложностью
- Или **1 длинный уровень** с чекпоинтами

---

## 8. UI/UX

### 8.1 HUD

```
┌─────────────────────────────────────────────┐
│ [HEAD HP ████████░░]  [BODY HP ██████░░░░]  │
│                                             │
│ [Текущая форма: ГОЛОВА/ТЕЛО — иконка]       │
│                                             │
│ [Dash CD: ●●●○]  [Parry CD: ●○]            │
│                                             │
│                         [Crosshair / cursor] │
│                                             │
│ [Marks: 3/5]                                │
└─────────────────────────────────────────────┘
```

- **HP обеих форм** отображаются всегда; HP активной формы — ярче/больше.
- **Иконка текущей формы** — в углу.
- **Кулдауны** — визуальные индикаторы.
- **Счётчик меток** — только в форме «Голова».
- **Прицел** — кастомный курсор, меняет форму в зависимости от режима.

### 8.2 Экраны

| Экран | Описание |
|---|---|
| Main Menu | Play, Quit |
| Pause | Resume, Restart, Quit |
| Game Over | Restart, Main Menu |
| Level Complete | Next Level / Main Menu |

---

## 9. Архитектура проекта

### 9.1 Принципы

1. **Clean Architecture** — разделение на слои: Domain (логика), Application (use cases), Infrastructure (Unity-специфичный код).
2. **Composition over Inheritance** — предпочитаем компоненты и композицию.
3. **ScriptableObject-based конфигурация** — все числовые параметры вынесены в SO, настраиваемые через Inspector.
4. **Event-driven коммуникация** — компоненты общаются через C# events и ScriptableObject events, минимум прямых ссылок.
5. **State Machine** — для управления состояниями игрока и врагов.
6. **Single Responsibility** — каждый класс/компонент отвечает за одну задачу.

### 9.2 Слои архитектуры

```
┌────────────────────────────────────────────────┐
│              Presentation Layer                │
│    (MonoBehaviours, Views, Animators, VFX)     │
├────────────────────────────────────────────────┤
│              Application Layer                 │
│   (Use Cases, Game Flow, State Machines)       │
├────────────────────────────────────────────────┤
│                Domain Layer                    │
│  (Pure C# classes: Health, Combat, Movement    │
│   logic, Configs as data structures)           │
├────────────────────────────────────────────────┤
│             Infrastructure Layer               │
│   (Input System, ScriptableObjects, Unity      │
│    Physics wrappers, Audio Manager)            │
└────────────────────────────────────────────────┘
```

### 9.3 Основные архитектурные паттерны

| Паттерн | Применение |
|---|---|
| **State Machine** | Состояния игрока (HeadForm, BodyForm); состояния врагов (Idle, Chase, Attack, Death); состояния игры (Menu, Playing, Paused, GameOver) |
| **Observer (Events)** | OnHealthChanged, OnFormSwitched, OnEnemyMarked, OnParrySuccess, OnPlayerDeath |
| **Strategy** | Разные стратегии поведения для форм игрока (IMovementStrategy, IShootingStrategy) |
| **Factory** | Создание снарядов (ProjectileFactory), создание врагов (EnemyFactory) |
| **Object Pool** | Пул снарядов для избежания GC-аллокаций |
| **Service Locator / DI** | Доступ к GameManager, AudioManager, CameraController (lightweight — через синглтон-сервисы или SO-ссылки) |

### 9.4 Диаграмма зависимостей (верхнеуровневая)

```
                    ┌──────────────┐
                    │ GameManager  │
                    │  (Game Flow) │
                    └──────┬───────┘
                           │
              ┌────────────┼────────────┐
              │            │            │
     ┌────────▼──────┐ ┌──▼──────┐ ┌───▼────────┐
     │ PlayerSystem  │ │ Enemy   │ │ UI Manager │
     │               │ │ System  │ │            │
     └───┬───┬───┬───┘ └────┬────┘ └────────────┘
         │   │   │          │
    ┌────▼┐ ┌▼──┐ ┌▼────┐  │
    │Move-│ │Com│ │Form │  │
    │ment │ │bat│ │Switc│  │
    └─────┘ └┬──┘ └─────┘  │
             │              │
        ┌────▼──────────────▼────┐
        │   Combat System        │
        │ (Projectiles, Parry,   │
        │  Damage, Health)       │
        └────────────────────────┘
```

### 9.5 Подробная архитектура игрока

```
PlayerController (MonoBehaviour)
├── References:
│   ├── PlayerConfig (ScriptableObject)
│   ├── Rigidbody2D
│   ├── Collider2D
│   └── SpriteRenderer / Animator
│
├── Components (composition):
│   ├── PlayerStateMachine
│   │   ├── HeadFormState : IPlayerFormState
│   │   └── BodyFormState : IPlayerFormState
│   │
│   ├── PlayerMovement
│   │   ├── IMovementStrategy CurrentStrategy
│   │   ├── HeadMovementStrategy (slow, no jump, no dash)
│   │   └── BodyMovementStrategy (fast, jump, dash)
│   │
│   ├── PlayerCombat
│   │   ├── ShootingHandler
│   │   ├── ParryHandler
│   │   └── MarkingHandler (only in HeadForm)
│   │
│   ├── PlayerHealth
│   │   ├── HealthComponent headHealth
│   │   └── HealthComponent bodyHealth
│   │
│   └── PlayerVision
│       ├── FullVisionHandler (HeadForm)
│       └── LimitedVisionHandler (BodyForm)
│
├── Events:
│   ├── event Action<FormType> OnFormSwitched
│   ├── event Action<float, float> OnHeadHealthChanged (current, max)
│   ├── event Action<float, float> OnBodyHealthChanged (current, max)
│   ├── event Action OnParrySuccess
│   ├── event Action OnDeath
│   └── event Action<GameObject> OnEnemyMarked
```

### 9.6 Подробная архитектура врагов

```
EnemyBase (MonoBehaviour, abstract)
├── References:
│   ├── EnemyConfig (ScriptableObject — базовый)
│   ├── Rigidbody2D
│   ├── Collider2D
│   └── SpriteRenderer / Animator
│
├── Components:
│   ├── HealthComponent
│   ├── EnemyStateMachine
│   │   ├── IdleState
│   │   ├── ChaseState (для Melee) / ShootState (для Ranged)
│   │   ├── AttackState
│   │   └── DeathState
│   ├── DetectionComponent (raycast / trigger-based)
│   └── MarkableComponent (реагирует на метки)
│
├── Events:
│   ├── event Action<float, float> OnHealthChanged
│   ├── event Action OnDeath
│   └── event Action OnMarked
│
├── Derived:
│   ├── MeleeEnemy : EnemyBase
│   │   └── MeleeAttackHandler
│   └── RangedEnemy : EnemyBase
│       └── RangedAttackHandler (использует ProjectileFactory)
```

### 9.7 Система стейт-машин

```csharp
// Общий интерфейс
public interface IState
{
    void Enter();
    void Update();
    void FixedUpdate();
    void Exit();
}

public class StateMachine
{
    private IState _currentState;

    public void ChangeState(IState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }

    public void Update() => _currentState?.Update();
    public void FixedUpdate() => _currentState?.FixedUpdate();
}
```

### 9.8 Система событий (Event Bus)

Для глобальной коммуникации используется простой Event Bus на основе ScriptableObject:

```csharp
[CreateAssetMenu(menuName = "Events/Void Event")]
public class VoidEventSO : ScriptableObject
{
    private readonly List<Action> _listeners = new();

    public void Register(Action listener) => _listeners.Add(listener);
    public void Unregister(Action listener) => _listeners.Remove(listener);
    public void Raise()
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
            _listeners[i]?.Invoke();
    }
}
```

Для типизированных событий:
```csharp
[CreateAssetMenu(menuName = "Events/Float Event")]
public class FloatEventSO : ScriptableObject
{
    private readonly List<Action<float>> _listeners = new();
    // ...
}
```

### 9.9 Object Pool для снарядов

```csharp
public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private ProjectileView prefab;
    [SerializeField] private int initialSize = 20;

    private Queue<ProjectileView> _pool;

    public ProjectileView Get() { /* ... */ }
    public void Return(ProjectileView projectile) { /* ... */ }
}
```

---

## 10. Структура папок

```
Assets/
├── _Project/
│   ├── Configs/                    # ScriptableObject instances
│   │   ├── Player/
│   │   │   ├── PlayerConfig.asset
│   │   │   ├── HeadFormConfig.asset
│   │   │   └── BodyFormConfig.asset
│   │   ├── Enemies/
│   │   │   ├── MeleeEnemyConfig.asset
│   │   │   └── RangedEnemyConfig.asset
│   │   ├── Projectiles/
│   │   │   ├── PlayerProjectileConfig.asset
│   │   │   └── EnemyProjectileConfig.asset
│   │   ├── Camera/
│   │   │   └── CameraConfig.asset
│   │   └── Vision/
│   │       └── VisionConfig.asset
│   │
│   ├── Scripts/
│   │   ├── Domain/                 # Pure C# — no MonoBehaviour
│   │   │   ├── Health/
│   │   │   │   └── HealthData.cs
│   │   │   ├── Combat/
│   │   │   │   ├── DamageCalculator.cs
│   │   │   │   └── ParryResult.cs
│   │   │   └── Enums/
│   │   │       ├── FormType.cs
│   │   │       └── EnemyState.cs
│   │   │
│   │   ├── Application/           # Use cases, state machines
│   │   │   ├── StateMachine/
│   │   │   │   ├── IState.cs
│   │   │   │   └── StateMachine.cs
│   │   │   ├── Player/
│   │   │   │   ├── States/
│   │   │   │   │   ├── HeadFormState.cs
│   │   │   │   │   └── BodyFormState.cs
│   │   │   │   ├── IMovementStrategy.cs
│   │   │   │   ├── HeadMovementStrategy.cs
│   │   │   │   └── BodyMovementStrategy.cs
│   │   │   ├── Enemies/
│   │   │   │   ├── States/
│   │   │   │   │   ├── EnemyIdleState.cs
│   │   │   │   │   ├── EnemyChaseState.cs
│   │   │   │   │   ├── EnemyAttackState.cs
│   │   │   │   │   ├── EnemyShootState.cs
│   │   │   │   │   └── EnemyDeathState.cs
│   │   │   │   └── DetectionLogic.cs
│   │   │   └── Game/
│   │   │       ├── GameStateMachine.cs
│   │   │       └── GameStates/
│   │   │           ├── MenuState.cs
│   │   │           ├── PlayingState.cs
│   │   │           ├── PausedState.cs
│   │   │           └── GameOverState.cs
│   │   │
│   │   ├── Infrastructure/        # Unity-specific, input, configs
│   │   │   ├── Input/
│   │   │   │   ├── IInputProvider.cs
│   │   │   │   └── UnityInputProvider.cs
│   │   │   ├── Configs/           # ScriptableObject definitions
│   │   │   │   ├── PlayerConfigSO.cs
│   │   │   │   ├── FormConfigSO.cs
│   │   │   │   ├── EnemyConfigSO.cs
│   │   │   │   ├── ProjectileConfigSO.cs
│   │   │   │   ├── CameraConfigSO.cs
│   │   │   │   └── VisionConfigSO.cs
│   │   │   ├── Events/
│   │   │   │   ├── VoidEventSO.cs
│   │   │   │   └── FloatEventSO.cs
│   │   │   ├── Factories/
│   │   │   │   ├── ProjectileFactory.cs
│   │   │   │   └── EnemyFactory.cs
│   │   │   └── Pools/
│   │   │       └── ProjectilePool.cs
│   │   │
│   │   └── Presentation/          # MonoBehaviours, Views
│   │       ├── Player/
│   │       │   ├── PlayerController.cs
│   │       │   ├── PlayerMovementView.cs
│   │       │   ├── PlayerCombatView.cs
│   │       │   ├── PlayerHealthView.cs
│   │       │   ├── PlayerVisionView.cs
│   │       │   ├── PlayerAnimationView.cs
│   │       │   └── MarkView.cs
│   │       ├── Enemies/
│   │       │   ├── EnemyView.cs
│   │       │   ├── MeleeEnemyView.cs
│   │       │   ├── RangedEnemyView.cs
│   │       │   └── MarkableView.cs
│   │       ├── Combat/
│   │       │   ├── ProjectileView.cs
│   │       │   └── ParryHitboxView.cs
│   │       ├── Camera/
│   │       │   └── CameraController.cs
│   │       ├── Vision/
│   │       │   ├── VisionController.cs
│   │       │   └── FogOfWarRenderer.cs
│   │       ├── UI/
│   │       │   ├── HUDView.cs
│   │       │   ├── HealthBarView.cs
│   │       │   ├── FormIndicatorView.cs
│   │       │   ├── CooldownIndicatorView.cs
│   │       │   ├── MarkCounterView.cs
│   │       │   ├── PauseMenuView.cs
│   │       │   ├── GameOverView.cs
│   │       │   └── MainMenuView.cs
│   │       └── Game/
│   │           └── GameManager.cs
│   │
│   ├── Art/
│   │   ├── Sprites/
│   │   │   ├── Player/
│   │   │   ├── Enemies/
│   │   │   ├── Projectiles/
│   │   │   ├── Environment/
│   │   │   ├── UI/
│   │   │   └── VFX/
│   │   ├── Animations/
│   │   │   ├── Player/
│   │   │   └── Enemies/
│   │   └── Tilemaps/
│   │       ├── Palettes/
│   │       └── Tiles/
│   │
│   ├── Audio/
│   │   ├── SFX/
│   │   └── Music/
│   │
│   ├── Prefabs/
│   │   ├── Player/
│   │   ├── Enemies/
│   │   ├── Projectiles/
│   │   ├── UI/
│   │   └── Environment/
│   │
│   ├── Scenes/
│   │   ├── MainMenu.unity
│   │   ├── Level_01.unity
│   │   ├── Level_02.unity
│   │   └── Level_03.unity
│   │
│   ├── Materials/
│   │   └── Vision/
│   │       └── FogOfWarMaterial.mat
│   │
│   └── Shaders/
│       └── FogOfWar.shader
│
└── Plugins/                        # Сторонние ассеты (если есть)
```

---

## 11. Описание основных классов

### 11.1 Domain Layer

#### `HealthData.cs`
```csharp
// Pure C# — хранит и управляет данными о здоровье
public class HealthData
{
    public float CurrentHP { get; private set; }
    public float MaxHP { get; private set; }
    public bool IsDead => CurrentHP <= 0;

    public event Action<float, float> OnHealthChanged; // current, max
    public event Action OnDied;

    public HealthData(float maxHP) { ... }
    public void TakeDamage(float damage) { ... }
    public void Heal(float amount) { ... }
}
```

#### `DamageCalculator.cs`
```csharp
public static class DamageCalculator
{
    public static float Calculate(float baseDamage, float multiplier)
    {
        return baseDamage * multiplier;
    }
}
```

#### `FormType.cs`
```csharp
public enum FormType { Head, Body }
```

### 11.2 Application Layer

#### `IState.cs` & `StateMachine.cs`
(Описаны выше в разделе 9.7)

#### `HeadFormState.cs`
```csharp
public class HeadFormState : IState
{
    // Настраивает движение (медленное), включает полную видимость,
    // включает возможность ставить метки, настраивает урон
    public void Enter() { ... }
    public void Update() { ... }
    public void Exit() { ... }
}
```

#### `BodyFormState.cs`
```csharp
public class BodyFormState : IState
{
    // Настраивает движение (быстрое + прыжок + dash),
    // включает ограниченную видимость, сниженный урон
    public void Enter() { ... }
    public void Update() { ... }
    public void Exit() { ... }
}
```

#### `IMovementStrategy.cs`
```csharp
public interface IMovementStrategy
{
    void Move(Rigidbody2D rb, float direction, float speed);
    bool CanJump { get; }
    void Jump(Rigidbody2D rb, float force);
    bool CanDash { get; }
    void Dash(Rigidbody2D rb, float direction, float distance, float duration);
}
```

### 11.3 Infrastructure Layer

#### `PlayerConfigSO.cs`
```csharp
[CreateAssetMenu(menuName = "Configs/Player Config")]
public class PlayerConfigSO : ScriptableObject
{
    [Header("Form Switch")]
    public float formSwitchCooldown = 0.5f;

    [Header("Parry")]
    public float parryWindow = 0.3f;
    public float parryCooldown = 0.5f;

    [Header("Forms")]
    public FormConfigSO headFormConfig;
    public FormConfigSO bodyFormConfig;
}
```

#### `FormConfigSO.cs`
```csharp
[CreateAssetMenu(menuName = "Configs/Form Config")]
public class FormConfigSO : ScriptableObject
{
    [Header("Health")]
    public int maxHP = 100;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public bool canJump = false;
    public float jumpForce = 12f;
    public bool canDash = false;
    public float dashDistance = 4f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1f;
    public bool dashInvincible = true;

    [Header("Combat")]
    public float shootDamage = 25f;
    public float shootRate = 0.4f;
    public float projectileSpeed = 15f;
    public float damageMultiplier = 1f;

    [Header("Marking (Head only)")]
    public float markDuration = 10f;
    public int maxMarks = 5;

    [Header("Vision")]
    public bool fullVision = true;
    public float innerVisionRadius = 3f;
    public float outerVisionRadius = 5f;
    public float outerVisionAlpha = 0.4f;
}
```

#### `EnemyConfigSO.cs`
```csharp
[CreateAssetMenu(menuName = "Configs/Enemy Config")]
public class EnemyConfigSO : ScriptableObject
{
    [Header("Health")]
    public int hp = 50;

    [Header("Movement")]
    public float moveSpeed = 4f;

    [Header("Detection")]
    public float detectionRange = 10f;
    public float detectionAngle = 180f;

    [Header("Attack")]
    public float attackDamage = 20f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;

    [Header("Ranged (if applicable)")]
    public float projectileSpeed = 10f;
    public float fireRate = 1.5f;
    public GameObject projectilePrefab;

    [Header("Behavior")]
    public bool canBeParried = true;
}
```

#### `IInputProvider.cs`
```csharp
public interface IInputProvider
{
    float HorizontalAxis { get; }
    bool JumpPressed { get; }
    bool DashPressed { get; }
    bool ShootPressed { get; }
    bool ParryPressed { get; } // ПКМ в форме Тело / F
    bool MarkPressed { get; }  // ПКМ в форме Голова
    bool SwitchFormPressed { get; }
    bool PausePressed { get; }
    Vector2 MouseWorldPosition { get; }
}
```

#### `UnityInputProvider.cs`
```csharp
public class UnityInputProvider : MonoBehaviour, IInputProvider
{
    // Реализация через Input.GetKey / новую Input System
    // Все клавиши настраиваемые через Inspector
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode dashKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode switchFormKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode parryKey = KeyCode.F;
    // ...
}
```

### 11.4 Presentation Layer

#### `PlayerController.cs` (главный координатор)
```csharp
public class PlayerController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private PlayerConfigSO config;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D mainCollider;

    [Header("Components")]
    [SerializeField] private PlayerMovementView movementView;
    [SerializeField] private PlayerCombatView combatView;
    [SerializeField] private PlayerHealthView healthView;
    [SerializeField] private PlayerVisionView visionView;
    [SerializeField] private PlayerAnimationView animationView;

    [Header("Input")]
    [SerializeField] private UnityInputProvider inputProvider;

    // State Machine
    private StateMachine _formStateMachine;
    private FormType _currentForm;

    // Инициализация, связывание компонентов, запуск стейт-машины
}
```

#### `ProjectileView.cs`
```csharp
public class ProjectileView : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D col;

    private float _damage;
    private float _speed;
    private Vector2 _direction;
    private bool _isReflected;
    private ProjectilePool _pool;

    public void Initialize(float damage, float speed, Vector2 direction,
                           LayerMask targetLayer, ProjectilePool pool) { ... }

    public void Reflect(float speedMultiplier) { ... }

    private void OnTriggerEnter2D(Collider2D other) { ... }
}
```

#### `CameraController.cs`
```csharp
public class CameraController : MonoBehaviour
{
    [SerializeField] private CameraConfigSO config;
    [SerializeField] private Transform target;

    private void LateUpdate()
    {
        // Следование за игроком + смещение к курсору (в HeadForm)
    }

    public void OnFormSwitched(FormType newForm)
    {
        // Изменение параметров камеры при переключении формы
    }
}
```

---

## 12. ScriptableObject конфиги

### 12.1 Полный список SO-ассетов

| Ассет | Тип | Назначение |
|---|---|---|
| `PlayerConfig.asset` | `PlayerConfigSO` | Общие параметры игрока |
| `HeadFormConfig.asset` | `FormConfigSO` | Параметры формы «Голова» |
| `BodyFormConfig.asset` | `FormConfigSO` | Параметры формы «Тело» |
| `MeleeEnemyConfig.asset` | `EnemyConfigSO` | Параметры ближнего врага |
| `RangedEnemyConfig.asset` | `EnemyConfigSO` | Параметры дальнего врага |
| `PlayerProjectileConfig.asset` | `ProjectileConfigSO` | Снаряд игрока |
| `EnemyProjectileConfig.asset` | `ProjectileConfigSO` | Снаряд врага |
| `CameraConfig.asset` | `CameraConfigSO` | Параметры камеры |
| `VisionConfig.asset` | `VisionConfigSO` | Параметры системы видимости |
| `OnPlayerDeath.asset` | `VoidEventSO` | Событие смерти |
| `OnFormSwitched.asset` | `VoidEventSO` | Событие смены формы |
| `OnParrySuccess.asset` | `VoidEventSO` | Событие парирования |

### 12.2 Преимущества подхода

- **Дизайнер** (художник) может балансировать игру без открытия кода
- Можно создать несколько вариантов конфигов для тестирования баланса
- Конфиги не теряются при ошибках в скриптах (отдельные ассеты)
- Можно менять параметры **в runtime** через Inspector

---

## 13. Roadmap разработки

### День 1: Фундамент

| Задача | Исполнитель | Приоритет |
|---|---|---|
| Настройка проекта Unity, структура папок, Git | Программист | 🔴 |
| Базовое движение игрока (влево/вправо/прыжок) | Программист | 🔴 |
| State Machine (HeadForm / BodyForm) | Программист | 🔴 |
| Переключение форм (Shift) | Программист | 🔴 |
| Концепт-арт, палитра цветов | Художник | 🔴 |
| Спрайты игрока (обе формы) — idle, run | Художник | 🔴 |
| Базовый тайлсет для прототипа | Художник | 🔴 |

### День 2: Боевая система

| Задача | Исполнитель | Приоритет |
|---|---|---|
| Система стрельбы + снаряды + пул | Программист | 🔴 |
| Система здоровья (обе формы) | Программист | 🔴 |
| Парирование | Программист | 🔴 |
| Враг-бегун (Melee) — AI + атака | Программист | 🟡 |
| Анимации стрельбы, парирования | Художник | 🔴 |
| Спрайты врагов | Художник | 🔴 |
| Система меток (Marking) | Художник-программист | 🟡 |

### День 3: Видимость, враги, уровни

| Задача | Исполнитель | Приоритет |
|---|---|---|
| Система видимости (туман для формы Тело) | Программист | 🔴 |
| Враг-стрелок (Ranged) — AI | Программист | 🟡 |
| Рывок (Dash) с i-frames | Программист | 🟡 |
| Дизайн и сборка уровней | Художник | 🔴 |
| HUD (HP, форма, кулдауны) | Художник-программист | 🔴 |
| Камера — следование + смещение к курсору | Программист | 🟡 |
| Ловушки (шипы) | Художник-программист | 🟢 |

### День 4: Полировка и билд

| Задача | Исполнитель | Приоритет |
|---|---|---|
| Звуковые эффекты (выстрел, парирование, урон, смерть, переключение) | Оба | 🟡 |
| Экраны меню, Game Over, пауза | Художник-программист | 🟡 |
| Баланс (настройка SO-конфигов) | Оба | 🔴 |
| Screen shake, hit stop, VFX частицы | Художник-программист | 🟢 |
| Тестирование, багфикс | Оба | 🔴 |
| Сборка билда (Windows) | Программист | 🔴 |
| Страница на itch.io, скриншоты, описание | Художник | 🔴 |

### Приоритеты
- 🔴 **Must Have** — без этого игра не работает
- 🟡 **Should Have** — сильно улучшает опыт
- 🟢 **Nice to Have** — если останется время

---

## Приложение A: Управление (сводная таблица)

| Действие | Клавиша | Форма «Голова» | Форма «Тело» |
|---|---|---|---|
| Движение | `A` / `D` | ✅ (медленно) | ✅ (быстро) |
| Прыжок | `Space` | ❌ | ✅ |
| Рывок | `LCtrl` | ❌ | ✅ |
| Стрельба | `ЛКМ` | ✅ (полный урон) | ✅ (сниженный урон) |
| Метка | `ПКМ` | ✅ | ❌ |
| Парирование | `ПКМ` / `F` | ✅ | ✅ |
| Смена формы | `Shift` | ✅ | ✅ |
| Пауза | `Esc` | ✅ | ✅ |

> **Примечание:** ПКМ контекстно переключается между меткой (Голова) и парированием (Тело). Альтернативно парирование доступно через `F` в обеих формах.

---

## Приложение B: Layer и Tag конвенции

| Layer | Описание |
|---|---|
| `Player` | Коллайдер игрока |
| `Enemy` | Коллайдеры врагов |
| `PlayerProjectile` | Снаряды игрока |
| `EnemyProjectile` | Снаряды врагов |
| `Environment` | Платформы, стены, пол |
| `Trap` | Ловушки |
| `Interactable` | Помечаемые объекты |

| Tag | Описание |
|---|---|
| `Player` | Игрок |
| `Enemy` | Враг |
| `Projectile` | Любой снаряд |
| `MeleeAttack` | Хитбокс ближней атаки врага |
| `ParryHitbox` | Хитбокс парирования игрока |
| `Trap` | Ловушка |

**Collision Matrix (важные пары):**

| | Player | Enemy | PlayerProj | EnemyProj | Environment | Trap |
|---|---|---|---|---|---|---|
| **Player** | — | ✅ | — | ✅ | ✅ | ✅ |
| **Enemy** | ✅ | — | ✅ | — | ✅ | — |
| **PlayerProj** | — | ✅ | — | — | ✅ | — |
| **EnemyProj** | ✅ | — | — | — | ✅ | — |
| **ParryHitbox** | — | ✅ | — | ✅ | — | — |

---

## Приложение C: Технические требования

| Параметр | Значение |
|---|---|
| Unity версия | 6000.3.14f1 (URP) |
| Rendering Pipeline | Universal Render Pipeline 2D |
| Разрешение пикселей | 16×16 или 32×32 на спрайт (определяется художником) |
| Target FPS | 60 |
| Целевое разрешение | 1920×1080, pixel-perfect |
| Pixel Per Unit | 16 или 32 |