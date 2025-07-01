PRAGMA foreign_keys = ON;

-- Таблица Tags (Категории/Теги)
CREATE TABLE IF NOT EXISTS Tags (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT UNIQUE NOT NULL
);

-- Таблица Tasks (Задачи)
CREATE TABLE IF NOT EXISTS Tasks (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    Description TEXT,
    Deadline TEXT,
    Completed BOOLEAN NOT NULL DEFAULT 0
);

-- Many-to-Many связь между задачами и тегами
CREATE TABLE IF NOT EXISTS TaskTags (
    TaskId INTEGER NOT NULL,
    TagId INTEGER NOT NULL,
    PRIMARY KEY (TaskId, TagId),
    FOREIGN KEY (TaskId) REFERENCES Tasks(Id) ON DELETE CASCADE,
    FOREIGN KEY (TagId) REFERENCES Tags(Id) ON DELETE CASCADE
);