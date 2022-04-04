using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestionsData", menuName = "QuestionsData", order = 1)]
public class QuizDataScriptableObject : ScriptableObject
{
    public string questionCategory;
    public List<Question> questions;
}
