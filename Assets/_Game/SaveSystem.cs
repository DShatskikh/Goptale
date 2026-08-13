using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

public static class SaveSystem
{
    public static void Save()
    {
        string saveFolderPath = Application.persistentDataPath;
        string currentSaveFileName = "save.txt";
        string filePath = Path.Combine(saveFolderPath, currentSaveFileName);
    
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                FieldInfo[] fields = Stats.Instance.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            
                foreach (var field in fields)
                {
                    object value = field.GetValue(Stats.Instance);
                    
                    if (field.FieldType.IsArray)
                    {
                        Array array = (Array)value;
                        if (array != null)
                        {
                            for (int i = 0; i < array.Length; i++)
                            {
                                writer.WriteLine($"{field.Name}{i}-{array.GetValue(i)?.ToString() ?? ""}");
                            }
                        }
                    }
                    else if (field.FieldType == typeof(List<string>))
                    {
                        // Специальная обработка для List<string>
                        List<string> list = (List<string>)value;
                        if (list != null)
                        {
                            // Сохраняем количество элементов, а затем сами элементы
                            writer.WriteLine($"{field.Name}_Count-{list.Count}");
                            for (int i = 0; i < list.Count; i++)
                            {
                                writer.WriteLine($"{field.Name}_{i}-{list[i] ?? ""}");
                            }
                        }
                        else
                        {
                            writer.WriteLine($"{field.Name}_Count-0");
                        }
                    }
                    else
                    {
                        string stringValue = value?.ToString() ?? "";
                        writer.WriteLine($"{field.Name}-{stringValue}");
                    }
                }
            }
            Debug.Log($"Сохранение выполнено: {filePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Ошибка сохранения: {e.Message}");
        }
    }

    public static void MetaSave()
    {
        string saveFolderPath = Application.persistentDataPath;
        string currentSaveFileName = "meta.txt";
        string filePath = Path.Combine(saveFolderPath, currentSaveFileName);
    
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                FieldInfo[] fields = Meta.Instance.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            
                foreach (var field in fields)
                {
                    object value = field.GetValue(Meta.Instance);
                    
                    if (field.FieldType.IsArray)
                    {
                        Array array = (Array)value;
                        if (array != null)
                        {
                            for (int i = 0; i < array.Length; i++)
                            {
                                writer.WriteLine($"{field.Name}{i}-{array.GetValue(i)?.ToString() ?? ""}");
                            }
                        }
                    }
                    else if (field.FieldType == typeof(List<string>))
                    {
                        // Специальная обработка для List<string>
                        List<string> list = (List<string>)value;
                        if (list != null)
                        {
                            // Сохраняем количество элементов, а затем сами элементы
                            writer.WriteLine($"{field.Name}_Count-{list.Count}");
                            for (int i = 0; i < list.Count; i++)
                            {
                                writer.WriteLine($"{field.Name}_{i}-{list[i] ?? ""}");
                            }
                        }
                        else
                        {
                            writer.WriteLine($"{field.Name}_Count-0");
                        }
                    }
                    else
                    {
                        string stringValue = value?.ToString() ?? "";
                        writer.WriteLine($"{field.Name}-{stringValue}");
                    }
                }
            }
            Debug.Log($"Сохранение выполнено: {filePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Ошибка сохранения: {e.Message}");
        }
    }
    
    public static Stats Load()
    {
        string saveFolderPath = Application.persistentDataPath;
        string currentSaveFileName = "save.txt";
        string filePath = Path.Combine(saveFolderPath, currentSaveFileName);
        
        if (!File.Exists(filePath))
        {
            Debug.Log("Файл сохранения не найден");
            return null;
        }
        
        try
        {
            Stats stats = new Stats();
            stats.Items = new string[8];
            stats.LayItemIDs = new List<string>(); // Инициализация списка
            
            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
            FieldInfo[] fields = typeof(Stats).GetFields(BindingFlags.Public | BindingFlags.Instance);
            
            Dictionary<string, FieldInfo> fieldDict = new Dictionary<string, FieldInfo>();
            foreach (var field in fields)
            {
                fieldDict[field.Name] = field;
            }
            
            // Временное хранилище для элементов списка
            Dictionary<string, List<string>> listItems = new Dictionary<string, List<string>>();
            Dictionary<string, int> listCounts = new Dictionary<string, int>();
            
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                
                int separatorIndex = line.IndexOf('-');
                if (separatorIndex == -1) continue;
                
                string key = line.Substring(0, separatorIndex);
                string value = line.Substring(separatorIndex + 1);

                string fieldName = string.Empty;
                
                // Проверяем, является ли это элементом списка
                if (key.EndsWith("_Count") && fieldDict.ContainsKey(key.Replace("_Count", "")))
                {
                    fieldName = key.Replace("_Count", "");
                    if (fieldDict[fieldName].FieldType == typeof(List<string>))
                    {
                        listCounts[fieldName] = int.Parse(value);
                        continue;
                    }
                }
                
                // Проверяем элемент списка по индексу
                int underscoreIndex = key.LastIndexOf('_');
                if (underscoreIndex > 0)
                {
                    fieldName = key.Substring(0, underscoreIndex);
                    string indexStr = key.Substring(underscoreIndex + 1);
                    
                    if (fieldDict.ContainsKey(fieldName) && 
                        fieldDict[fieldName].FieldType == typeof(List<string>) &&
                        int.TryParse(indexStr, out int index))
                    {
                        if (!listItems.ContainsKey(fieldName))
                            listItems[fieldName] = new List<string>();
                        
                        // Заполняем список с запасом по индексу
                        while (listItems[fieldName].Count <= index)
                            listItems[fieldName].Add("");
                        
                        listItems[fieldName][index] = value;
                        continue;
                    }
                }
                
                // Обработка обычных полей и массивов
                fieldName = key;
                int arrayIndex = -1;
                
                for (int i = key.Length - 1; i >= 0; i--)
                {
                    if (char.IsDigit(key[i]))
                        continue;
                    else
                    {
                        if (i < key.Length - 1)
                        {
                            string indexStr = key.Substring(i + 1);
                            if (int.TryParse(indexStr, out arrayIndex))
                            {
                                fieldName = key.Substring(0, i + 1);
                            }
                        }
                        break;
                    }
                }
                
                if (!fieldDict.ContainsKey(fieldName))
                {
                    Debug.LogWarning($"Поле '{fieldName}' не найдено в классе Stats");
                    continue;
                }
                
                FieldInfo field = fieldDict[fieldName];
                object parsedValue = null;
                
                if (field.FieldType.IsArray)
                {
                    Array array = (Array)field.GetValue(stats);
                    if (array == null)
                    {
                        array = Array.CreateInstance(field.FieldType.GetElementType(), 8);
                        field.SetValue(stats, array);
                    }
                    
                    Type elementType = field.FieldType.GetElementType();
                    parsedValue = ParseValue(value, elementType);
                    
                    if (arrayIndex >= 0 && arrayIndex < array.Length)
                    {
                        array.SetValue(parsedValue, arrayIndex);
                    }
                }
                else
                {
                    parsedValue = ParseValue(value, field.FieldType);
                    field.SetValue(stats, parsedValue);
                }
            }
            
            // Восстанавливаем списки
            foreach (var kvp in listItems)
            {
                if (fieldDict.ContainsKey(kvp.Key) && fieldDict[kvp.Key].FieldType == typeof(List<string>))
                {
                    fieldDict[kvp.Key].SetValue(stats, kvp.Value);
                }
            }
            
            Debug.Log($"Загрузка выполнена: {filePath}");
            return stats;
        }
        catch (Exception e)
        {
            Debug.LogError($"Ошибка загрузки: {e.Message}");
            return null;
        }
    }

    public static Meta MetaLoad()
    {
        string saveFolderPath = Application.persistentDataPath;
        string currentSaveFileName = "meta.txt";
        string filePath = Path.Combine(saveFolderPath, currentSaveFileName);
        
        Meta meta = Meta.GetDefault();
        
        if (!File.Exists(filePath))
        {
            Debug.Log("Файл сохранения не найден");
            return meta;
        }
        
        try
        {
            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
            FieldInfo[] fields = typeof(Stats).GetFields(BindingFlags.Public | BindingFlags.Instance);
            
            Dictionary<string, FieldInfo> fieldDict = new Dictionary<string, FieldInfo>();
            foreach (var field in fields)
            {
                fieldDict[field.Name] = field;
            }
            
            // Временное хранилище для элементов списка
            Dictionary<string, List<string>> listItems = new Dictionary<string, List<string>>();
            Dictionary<string, int> listCounts = new Dictionary<string, int>();
            
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                
                int separatorIndex = line.IndexOf('-');
                if (separatorIndex == -1) continue;
                
                string key = line.Substring(0, separatorIndex);
                string value = line.Substring(separatorIndex + 1);

                string fieldName = string.Empty;
                
                // Проверяем, является ли это элементом списка
                if (key.EndsWith("_Count") && fieldDict.ContainsKey(key.Replace("_Count", "")))
                {
                    fieldName = key.Replace("_Count", "");
                    if (fieldDict[fieldName].FieldType == typeof(List<string>))
                    {
                        listCounts[fieldName] = int.Parse(value);
                        continue;
                    }
                }
                
                // Проверяем элемент списка по индексу
                int underscoreIndex = key.LastIndexOf('_');
                if (underscoreIndex > 0)
                {
                    fieldName = key.Substring(0, underscoreIndex);
                    string indexStr = key.Substring(underscoreIndex + 1);
                    
                    if (fieldDict.ContainsKey(fieldName) && 
                        fieldDict[fieldName].FieldType == typeof(List<string>) &&
                        int.TryParse(indexStr, out int index))
                    {
                        if (!listItems.ContainsKey(fieldName))
                            listItems[fieldName] = new List<string>();
                        
                        // Заполняем список с запасом по индексу
                        while (listItems[fieldName].Count <= index)
                            listItems[fieldName].Add("");
                        
                        listItems[fieldName][index] = value;
                        continue;
                    }
                }
                
                // Обработка обычных полей и массивов
                fieldName = key;
                int arrayIndex = -1;
                
                for (int i = key.Length - 1; i >= 0; i--)
                {
                    if (char.IsDigit(key[i]))
                        continue;
                    else
                    {
                        if (i < key.Length - 1)
                        {
                            string indexStr = key.Substring(i + 1);
                            if (int.TryParse(indexStr, out arrayIndex))
                            {
                                fieldName = key.Substring(0, i + 1);
                            }
                        }
                        break;
                    }
                }
                
                if (!fieldDict.ContainsKey(fieldName))
                {
                    Debug.LogWarning($"Поле '{fieldName}' не найдено в классе Meta");
                    continue;
                }
                
                FieldInfo field = fieldDict[fieldName];
                object parsedValue = null;
                
                if (field.FieldType.IsArray)
                {
                    Array array = (Array)field.GetValue(meta);
                    if (array == null)
                    {
                        array = Array.CreateInstance(field.FieldType.GetElementType(), 8);
                        field.SetValue(meta, array);
                    }
                    
                    Type elementType = field.FieldType.GetElementType();
                    parsedValue = ParseValue(value, elementType);
                    
                    if (arrayIndex >= 0 && arrayIndex < array.Length)
                    {
                        array.SetValue(parsedValue, arrayIndex);
                    }
                }
                else
                {
                    parsedValue = ParseValue(value, field.FieldType);
                    field.SetValue(meta, parsedValue);
                }
            }
            
            // Восстанавливаем списки
            foreach (var kvp in listItems)
            {
                if (fieldDict.ContainsKey(kvp.Key) && fieldDict[kvp.Key].FieldType == typeof(List<string>))
                {
                    fieldDict[kvp.Key].SetValue(meta, kvp.Value);
                }
            }
            
            Debug.Log($"Загрузка выполнена: {filePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Ошибка загрузки: {e.Message}");
        }
        
        return meta;
    }
    
    public static void DeleteSave()
    {
        string saveFolderPath = Application.persistentDataPath;
        string currentSaveFileName = "save.txt";
        string filePath = Path.Combine(saveFolderPath, currentSaveFileName);
        
        if (!File.Exists(filePath))
        {
            Debug.Log("Файл сохранения не найден");
            return;
        }
        
        Debug.Log("Файл удален");
        File.Delete(filePath);
    }
    
    private static object ParseValue(string value, Type targetType)
    {
        if (targetType == typeof(int))
            return int.Parse(value);
        else if (targetType == typeof(float))
        {
            string normalizedValue = value.Replace(',', '.');
            return float.Parse(normalizedValue, System.Globalization.CultureInfo.InvariantCulture);
        }
        else if (targetType == typeof(string))
            return value;
        else if (targetType == typeof(bool))
            return bool.Parse(value);
        else if (targetType == typeof(Vector2))
        {
            string cleanValue = value.Trim('(', ')');
            string[] parts = cleanValue.Split(',');
            if (parts.Length == 2)
            {
                float x = float.Parse(parts[0].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                float y = float.Parse(parts[1].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                return new Vector2(x, y);
            }
            return Vector2.zero;
        }
        else
        {
            var converter = System.ComponentModel.TypeDescriptor.GetConverter(targetType);
            if (converter.CanConvertFrom(typeof(string)))
            {
                return converter.ConvertFromInvariantString(value);
            }
        }
    
        Debug.LogWarning($"Неизвестный тип: {targetType}");
        return null;
    }

    public static bool IsSave()
    {
        return Load() != null;
    }
}