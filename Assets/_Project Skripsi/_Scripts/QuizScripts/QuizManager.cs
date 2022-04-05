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

        player = FindObjectOfType<PlayerManager>();
        // DontDestroyOnLoad(this);
    }
    [SerializeField] QuizUI quizUI;
    [SerializeField] List<QuizDataScriptableObject> quizDatas;

    public List<Question> questions;
    [SerializeField] private Question selectedQuestion;
    PhotonView PV;
    public PlayerManager player;
    [SerializeField] GameSettings gameSettings;


    // Start is called before the first frame updates
    void Start()
    {
        PV = GetComponent<PhotonView>();
        // AddQuestions();
        RPC_AddQuestions();
        SelectQuestion();
    }

    void AddQuestions()
    {
        PV.RPC("RPC_AddQuestions", RpcTarget.All);
    }

    public void RPC_AddQuestions()
    {
        for (int i = 0; i < quizDatas.Count; i++)
        {
            foreach (var question in quizDatas[i].questions)
            {
                question.questionCategory = quizDatas[i].questionCategory;
                question.questionScore = gameSettings.defaultScore;
                questions.Add(question); //= quizData.questions;
            }
        }
    }

    public void SelectQuestion()
    {
        PV.RPC("RPC_SelectQuestion", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_SelectQuestion()
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
        // print($"Total character {selectedQuestion.questionInfo.Length}");
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
