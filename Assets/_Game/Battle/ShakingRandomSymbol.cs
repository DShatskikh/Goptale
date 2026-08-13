using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class ShakingRandomSymbol : MonoBehaviour
{
    [SerializeField]
    private float _force1 = 50, _force2 = 0.015f, _timer1 = 1, _timer2 = 1, _minFrequency = 2, _maxFrequency = 5;

    private void OnEnable()
    {
        StartCoroutine(AwaitEffect());
    }

    private IEnumerator AwaitEffect()
    {
        var label = GetComponent<TMP_Text>();
        yield return new WaitForSeconds(Random.Range(0, _maxFrequency));
        
        while (true)
        {
            // Принудительно обновляем меш
            label.ForceMeshUpdate();
            var mesh = label.mesh;
            var vertices = mesh.vertices;
            
            // Проверяем, есть ли символы
            if (label.text.Length == 0)
            {
                yield return new WaitForSeconds(_timer2);
                continue;
            }
            
            var index = Random.Range(0, label.text.Length);
            var timer = _timer1;
            
            // Обновляем информацию о символах
            label.ForceMeshUpdate();
            TMP_CharacterInfo c1 = label.textInfo.characterInfo[index];
            int index3 = c1.vertexIndex;
            
            // Сохраняем начальные позиции вершин
            var startVertices = new[]
            {
                vertices[index3],
                vertices[index3 + 1],
                vertices[index3 + 2],
                vertices[index3 + 3]
            };
            
            while (timer > 0)
            {
                // Проверяем валидность индекса
                if (index >= label.text.Length)
                    break;
                
                // Обновляем меш заново
                label.ForceMeshUpdate();
                mesh = label.mesh;
                vertices = mesh.vertices;
                
                TMP_CharacterInfo c = label.textInfo.characterInfo[index];
                int index2 = c.vertexIndex;
                
                // Генерируем смещение
                var time = Time.time;
                Vector3 offset = new Vector2(0, Mathf.Sin(time * _force1)) * _force2;
                
                // Применяем смещение
                vertices[index2] = startVertices[0] + offset;
                vertices[index2 + 1] = startVertices[1] + offset;
                vertices[index2 + 2] = startVertices[2] + offset;
                vertices[index2 + 3] = startVertices[3] + offset;
                
                // Обновляем меш
                mesh.vertices = vertices;
                mesh.RecalculateBounds(); // Важно!
                
                timer -= Time.deltaTime;
                yield return null;
            }
            
            // Возвращаем вершины в исходное положение
            label.ForceMeshUpdate();
            mesh = label.mesh;
            vertices = mesh.vertices;
            
            vertices[index3] = startVertices[0];
            vertices[index3 + 1] = startVertices[1];
            vertices[index3 + 2] = startVertices[2];
            vertices[index3 + 3] = startVertices[3];
            
            mesh.vertices = vertices;
            mesh.RecalculateBounds();
            
            yield return new WaitForSeconds(Random.Range(_minFrequency, _maxFrequency));
        }
    }
}