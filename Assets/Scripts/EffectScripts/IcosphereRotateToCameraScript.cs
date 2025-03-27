using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcosphereRotateToCameraScript : MonoBehaviour
{
    [SerializeField] private Transform transformIcoSphere;
    public Transform target; // Целевой объект, на который должен смотреть дочерний объект
    public Transform childObject; // Дочерний объект сферы
    public float rotationSpeed = 5f; // Скорость поворота

    void Update()
    {
        if (target == null || childObject == null) return;

        // Направление от дочернего объекта к цели
        Vector3 directionToTarget = target.position - childObject.position;
        if (directionToTarget == Vector3.zero) return;

        // Создание поворота, чтобы ось Y дочернего объекта смотрела на цель
        Quaternion targetRotation = Quaternion.FromToRotation(-childObject.up, directionToTarget) * transformIcoSphere.rotation;

        // Плавное вращение всей сферы
        transformIcoSphere.rotation = Quaternion.Slerp(transformIcoSphere.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}

