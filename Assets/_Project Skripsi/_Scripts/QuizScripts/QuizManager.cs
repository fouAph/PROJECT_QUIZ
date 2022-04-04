using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using WF;

public class QuizManager : MonoBehaviourPunCallbacks
{
    public static QuizManager instance;
    private void Awake()
    {
        if (instance != null) DestroyImmediate(instance);
        instance = this;

        player = GetComponent<PlayerManager>();
        // DontDestroyOnLoad(this);
    }
    [SerializeField] QuizUI quizUI;
    [SerializeField] List<QuizDataScriptableObject> quizDatas;

    public List<Question> questions;
    // public float timerInSecond = 5;
    [SerializeField] private Question selectedQuestion;
    // float currentTime;
    // PhotonView PV;
    public PlayerManager player;

    // GameManager gameManager => GameManager.instance;

    // Start is called before the first frame updates
    void Start()
    {
        // print(PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>().name);
        // player = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
        foreach (var quizData in quizDatas)
        {
            foreach (var question in quizData.questions)
            {
                question.questionCategory = quizData.questionCategory;
                questions.Add(question); //= quizData.questions;
            }
        }
        SelectQuestion();
    }


    private void SelectQuestion()
    {
        int val = Random.Range(0, questions.Count);
        if (questions.Count > 0)
        {
            selectedQuestion = questions[val];
        }
        else
        {
            print($"No more Question, You answer it All");
        }
        quizUI.SetQuestion(selectedQuestion);
        // if (selectedQuestion.questionInfo.Length > 70)
        // {
        //     currentTime = 70;
        // }
        print($"Total character {selectedQuestion.questionInfo.Length}");
        // currentTime = timerInSecond;

        if (questions.Count > 0)
            questions.RemoveAt(val);
    }

    public bool Answer(string answered)
    {
        bool correctAnswer = false;
        if (answered == selectedQuestion.correctAnswer)
        {
            correctAnswer = true;
            //Set score based on the QuestionScore
            SetScore(player);
        }
        else
        {
            // no
        }

        Invoke("SelectQuestion", 0.4f);
        return correctAnswer;
    }

    private void GameOver()
    {

    }

    private void SetScore(PlayerManager player)
    {
        player.playerScore += selectedQuestion.questionScore;
    }

}
[System.Serializable]
public class Question
{
    public string questionInfo;
    public string questionCategory { get; set; }
    public QuestionType questionType;
    public int questionScore;
    public Sprite questionImage;
    public AudioClip questionAudioClip;
    public UnityEngine.Video.VideoClip questionVideo;
    public List<string> options;
    public string correctAnswer;

}

public enum QuestionType { Text, Image, Video, Audio }
