using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PacStudent : MonoBehaviour
{
    private Animator animatorController;
    [SerializeField] private GameObject PacStudentSpriteGO;
    public bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        animatorController = PacStudentSpriteGO.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            animatorController.SetTrigger("DeadTrigger");
        }
    }
}
