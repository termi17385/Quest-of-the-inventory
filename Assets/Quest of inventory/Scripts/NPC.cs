using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[HideMonoScript]
public class NPC : MonoBehaviour
{
    [SerializeField] private GameObject indicator;
    [SerializeField] private Transform marker;

    private PlayerController pController;

    private void Awake() => pController = FindObjectOfType<PlayerController>();
    private void Update() => DetectPlayer(pController.lookingAtNpc);
    public void DetectPlayer(bool _inSight) => indicator.SetActive(_inSight);
}
