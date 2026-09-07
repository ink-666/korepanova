"""
Лабораторная работа №3: Чтение данных из Excel
Студент: ink-666
Дата: 17.03.2026
"""

import pandas as pd
import numpy as np

print("=" * 60)
print("ЛАБОРАТОРНАЯ РАБОТА №3: ЧТЕНИЕ ДАННЫХ ИЗ EXCEL")
print("=" * 60)

# ==========================================================
# 1. БАЗОВОЕ ЧТЕНИЕ ФАЙЛА
# ==========================================================
print("\n" + "=" * 60)
print("1. БАЗОВОЕ ЧТЕНИЕ ФАЙЛА")
print("=" * 60)

# Читаем первый лист (по умолчанию)
print("\n>>> Чтение первого листа (по умолчанию):")
df_default = pd.read_excel("../data.xlsx")
print(df_default)
print(f"\nФорма данных: {df_default.shape}")
print(f"Типы данных:\n{df_default.dtypes}")

# ==========================================================
# 2. ЧТЕНИЕ КОНКРЕТНОГО ЛИСТА
# ==========================================================
print("\n" + "=" * 60)
print("2. ЧТЕНИЕ КОНКРЕТНОГО ЛИСТА")
print("=" * 60)

# Чтение по имени листа
print("\n>>> Чтение листа 'Предметы' по имени:")
df_subjects = pd.read_excel("../data.xlsx", sheet_name="Предметы")
print(df_subjects)

# Чтение по индексу листа (0 - первый, 1 - второй, 2 - третий)
print("\n>>> Чтение третьего листа по индексу (sheet_name=2):")
df_grades = pd.read_excel("../data.xlsx", sheet_name=2)
print(df_grades)

# ==========================================================
# 3. ПРОПУСК СТРОК (skiprows)
# ==========================================================
print("\n" + "=" * 60)
print("3. ПРОПУСК СТРОК (skiprows)")
print("=" * 60)

# Пропускаем первую строку (например, если там заголовок не нужен)
print("\n>>> Пропускаем 1 строку (skiprows=1):")
df_skip1 = pd.read_excel("../data.xlsx", sheet_name="Студенты", skiprows=1)
print(df_skip1)

# Пропускаем первые 2 строки
print("\n>>> Пропускаем 2 строки (skiprows=2):")
df_skip2 = pd.read_excel("../data.xlsx", sheet_name="Студенты", skiprows=2)
print(df_skip2)

# ==========================================================
# 4. ИСПОЛЬЗОВАНИЕ ЗАГОЛОВКОВ (header)
# ==========================================================
print("\n" + "=" * 60)
print("4. ИСПОЛЬЗОВАНИЕ ЗАГОЛОВКОВ (header)")
print("=" * 60)

# Без заголовков
print("\n>>> Без заголовков (header=None):")
df_no_header = pd.read_excel("../data.xlsx", sheet_name="Студенты", header=None)
print(df_no_header)

# Свои названия столбцов
print("\n>>> Свои названия столбцов (names=...):")
custom_columns = ["Номер", "ФИО", "Лет", "Балл"]
df_custom_header = pd.read_excel("../data.xlsx", sheet_name="Студенты", 
                                  header=0, names=custom_columns)
print(df_custom_header)

# ==========================================================
# 5. ВЫБОР КОНКРЕТНЫХ СТОЛБЦОВ (usecols)
# ==========================================================
print("\n" + "=" * 60)
print("5. ВЫБОР КОНКРЕТНЫХ СТОЛБЦОВ (usecols)")
print("=" * 60)

# По номерам столбцов
print("\n>>> Только первые два столбца (usecols=[0,1]):")
df_cols_by_index = pd.read_excel("../data.xlsx", sheet_name="Студенты", 
                                  usecols=[0, 1])
print(df_cols_by_index)

# По названиям
print("\n>>> Только столбцы 'Имя' и 'Средний балл' (usecols=['Имя','Средний балл']):")
df_cols_by_name = pd.read_excel("../data.xlsx", sheet_name="Студенты", 
                                 usecols=['Имя', 'Средний балл'])
print(df_cols_by_name)

# Диапазон столбцов
print("\n>>> Столбцы A:C (usecols='A:C'):")
df_cols_range = pd.read_excel("../data.xlsx", sheet_name="Студенты", 
                               usecols='A:C')
print(df_cols_range)

# ==========================================================
# 6. ОГРАНИЧЕНИЕ КОЛИЧЕСТВА СТРОК (nrows)
# ==========================================================
print("\n" + "=" * 60)
print("6. ОГРАНИЧЕНИЕ КОЛИЧЕСТВА СТРОК (nrows)")
print("=" * 60)

print("\n>>> Только первые 3 строки (nrows=3):")
df_first3 = pd.read_excel("../data.xlsx", sheet_name="Студенты", nrows=3)
print(df_first3)

# ==========================================================
# 7. ЗАДАНИЕ ТИПОВ ДАННЫХ (dtype)
# ==========================================================
print("\n" + "=" * 60)
print("7. ЗАДАНИЕ ТИПОВ ДАННЫХ (dtype)")
print("=" * 60)

print("\n>>> Явное задание типов данных:")
dtype_dict = {'ID': str, 'Возраст': float, 'Средний балл': str}
df_with_dtypes = pd.read_excel("../data.xlsx", sheet_name="Студенты", 
                                dtype=dtype_dict)
print(df_with_dtypes)
print(f"\nТипы после приведения:\n{df_with_dtypes.dtypes}")

# ==========================================================
# 8. ЧТЕНИЕ НЕСКОЛЬКИХ ЛИСТОВ ОДНОВРЕМЕННО
# ==========================================================
print("\n" + "=" * 60)
print("8. ЧТЕНИЕ НЕСКОЛЬКИХ ЛИСТОВ ОДНОВРЕМЕННО")
print("=" * 60)

print("\n>>> Чтение листов 'Студенты' и 'Предметы':")
sheets_dict = pd.read_excel("../data.xlsx", 
                            sheet_name=['Студенты', 'Предметы'],
                            skiprows=0)

print(f"\nТип результата: {type(sheets_dict)}")
print(f"Ключи словаря: {list(sheets_dict.keys())}")

print("\n>>> Данные из листа 'Студенты':")
print(sheets_dict['Студенты'].head())

print("\n>>> Данные из листа 'Предметы':")
print(sheets_dict['Предметы'].head())

# ==========================================================
# 9. ЧТЕНИЕ ВСЕХ ЛИСТОВ
# ==========================================================
print("\n" + "=" * 60)
print("9. ЧТЕНИЕ ВСЕХ ЛИСТОВ")
print("=" * 60)

print("\n>>> Чтение всех листов (sheet_name=None):")
all_sheets = pd.read_excel("../data.xlsx", sheet_name=None)

for sheet_name, df in all_sheets.items():
    print(f"\nЛист: {sheet_name}")
    print(f"Размер: {df.shape}")
    print(df.head(2))

# ==========================================================
# 10. КОМБИНИРОВАНИЕ ПАРАМЕТРОВ
# ==========================================================
print("\n" + "=" * 60)
print("10. КОМБИНИРОВАНИЕ ПАРАМЕТРОВ")
print("=" * 60)

print("\n>>> Комбинация параметров:")
print("    - Лист: 'Оценки'")
print("    - Пропуск 0 строк")
print("    - Только столбцы A, B, C")
print("    - Первые 2 строки")
print("    - Свои названия столбцов\n")

df_combined = pd.read_excel("../data.xlsx", 
                            sheet_name='Оценки',
                            skiprows=0,
                            usecols='A:C',
                            nrows=2,
                            names=['Student_ID', 'Subject_Code', 'Grade'])

print(df_combined)

print("\n" + "=" * 60)
print("ПРОГРАММА PYTHON ЗАВЕРШЕНА")
print("=" * 60)