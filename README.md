# 🧿 The Green Eyes In Dark

A mysterious survival maze game where danger hides in the shadows.

---

## 📖 Overview

| Item           | Description                           |
|----------------|---------------------------------------|
| 🎮 Engine       | Unity (C#)                            |
| 📱 Platform     | Android / PC                          |
| 🧩 Genre        | Maze, Horror, Survival                |
| 🌘 Theme        | Darkness, Mystery, Monsters           |
| 🧠 Inspired by  | Tabletop game "Labyrinth with Green Eyes in the Dark" |

---

## 🕹️ Gameplay

You awaken in complete darkness inside a strange, shifting maze.  
Your only goal: **survive**.

### 🎯 Objectives

- 🔍 Explore and reveal the maze tile by tile
- 🧿 Avoid monsters known as “The Green Eyes”
- 💰 Collect coins and power-ups
- ✨ Use teleporters to escape
- 🏃‍♂️ Find the exit before it’s too late

---

## 🎮 Controls

| Action           | Android Controls        | PC Controls           |
|------------------|-------------------------|------------------------|
| Move             | Swipe in any direction  | Arrow Keys (← ↑ ↓ →)  |
| Alternate Move   | Accelerometer tilt      | —                      |
| Open Settings    | On-screen ⚙️ button     | Mouse / Escape key    |

> Fully supports touchscreen and motion input.

---

## 🧰 Features

- Dynamic, procedurally uncovered maze
- Smart enemies with hearing-based detection
- Smooth animated settings menu
- Collectibles: coins, power-ups, teleporters
- Mysterious empty zones and hidden paths

---

## 🛠️ Sample Code

```csharp
// Animated settings menu with sliding panel
public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private RectTransform menuPanel;
    [SerializeField] private Button toggleButton;
    [SerializeField] private float animationSpeed = 5f;
    // ...
}
The menu slides in and out using a toggle button — optimized for mobile UI.

🚀 How to Play
Android:
Build APK via Unity

Install on your device

Launch and explore the darkness

PC (Editor):
Clone or download this repository

Open the project with Unity Hub

Run from editor using the ▶️ Play button

📌 Future Plans
🧭 Mini-map system

🔦 Flashlight or limited vision radius

🧠 Smarter monster AI

🧩 Unlockable difficulty modes

👤 Author
Mikhail Martemyanov — a solo developer bringing a handmade tabletop game to life through Unity.
Feedback welcome!

📜 License
This project is non-commercial and based on personal tabletop rules.


# 🧿 The Green Eyes In Dark

Мистическая игра-лабиринт на выживание, где опасность прячется в темноте.

---

## 📖 Обзор

| Пункт          | Описание                                      |
|----------------|-----------------------------------------------|
| 🎮 Движок       | Unity (C#)                                    |
| 📱 Платформа    | Android / ПК                                  |
| 🧩 Жанр         | Лабиринт, Ужасы, Выживание                    |
| 🌘 Тема         | Тьма, Тайны, Монстры                          |
| 🧠 Основано на  | Настольной игре "Лабиринт с зелёными глазами в темноте" |

---

## 🕹️ Игровой процесс

Вы очнулись в полной темноте внутри странного, меняющегося лабиринта.  
Ваша цель — **выжить**.

### 🎯 Цели

- 🔍 Исследуй лабиринт, открывая его по частям
- 🧿 Избегай монстров — "Зелёных Глаз"
- 💰 Собирай монеты и усиления
- ✨ Используй телепорты
- 🏃‍♂️ Найди выход до того, как станет слишком поздно

---

## 🎮 Управление

| Действие        | Android                    | ПК                     |
|------------------|----------------------------|------------------------|
| Движение         | Свайп в нужную сторону     | Клавиши ← ↑ ↓ →        |
| Альтернатива     | Наклон телефона (аксель)   | —                      |
| Меню настроек    | Кнопка ⚙️ на экране        | Мышь / клавиша Esc     |

> Поддержка сенсора движения и жестов для мобильных устройств.

---

## 🧰 Особенности

- Пошаговое открытие карты
- Умные враги, реагирующие на звук
- Анимированное меню настроек
- Монеты, телепорты, скрытые элементы
- Пустые зоны и ловушки

---

## 🛠️ Пример кода

```csharp
// Анимированное меню настроек с кнопкой
public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private RectTransform menuPanel;
    [SerializeField] private Button toggleButton;
    [SerializeField] private float animationSpeed = 5f;
    // ...
}
Меню плавно открывается и закрывается кнопкой. Оптимизировано для мобильных интерфейсов.

🚀 Как играть
Android:
Собери APK через Unity

Установи на устройство

Запускай и исследуй тьму

ПК (в редакторе):
Склонируй или скачай репозиторий

Открой через Unity Hub

Нажми ▶️ Play

📌 Будущие планы
🧭 Мини-карта

🔦 Ограниченный свет (фонарик)

🧠 Продвинутый ИИ монстров

🧩 Разблокируемые режимы

👤 Автор
Михаил Мартемьянов — соло-разработчик, воплотивший настольную игру в цифровом формате с помощью Unity.
Буду рад обратной связи!

📜 Лицензия
Проект некоммерческий, основан на авторских правилах настольной игры.