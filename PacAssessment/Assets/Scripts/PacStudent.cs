using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudent : MonoBehaviour
{

    public Animator animatorController;
    [SerializeField] private GameObject PacStudentGO;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animatorController.SetTrigger("DeadTrigger");
    }
}
