using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class QuizUI : MonoBehaviour
{
    public QuizManager quizManager;

    public TMP_Text questionCategoryText;
    public TMP_Text questionText;
    public TMP_Text playerScoreText;
    public TMP_Text questionScoreText;

    public Image questionImage;
    public UnityEngine.Video.VideoPlayer questionVideo;
    public AudioSource questionAudio;
    public List<Button> optionButtons;
    public Color correctCol, wrongCol, normalCol;

    public Question question;
    public bool answered;

    float audioLength;

    private void Awake()
    {
        for (int i = 0; i < optionButtons.Count; i++)
        {
            Button localBtn = optionButtons[i];
            localBtn.onClick.AddListener(() => OnClick(localBtn));
        }
    }

    public void SetQuestion(Question question)
    {
        this.question = question;

        switch (question.questionType)
        {
            case QuestionType.Text:
                questionImage.transform.parent.gameObject.SetActive(false);
                break;

            case QuestionType.Image:
                ImageHolder();
                questionImage.gameObject.SetActive(true);
                questionImage.sprite = question.questionImage;
                break;

            case QuestionType.Audio:
                ImageHolder();
                questionAudio.gameObject.SetActive(true);
                audioLength = question.questionAudioClip.length;
                break;

            case QuestionType.Video:
                ImageHolder();
                questionVideo.gameObject.SetActive(true);
                questionVideo.clip = question.questionVideo;
                questionVideo.Play();
                break;
        }

        questionCategoryText.text = question.questionCategory;
        questionText.text = question.questionInfo;
        List<string> answerList = ShuffleList.ShuffleListItems<string>(question.options);

        for (int i = 0; i < optionButtons.Count; i++)
        {
            optionButtons[i].GetComponentInChildren<TMP_Text>().text = answerList[i];
            optionButtons[i].name = answerList[i];
            optionButtons[i].image.color = normalCol;
        }

        //TODO Uncomment this line
        questionScoreText.text = $"Question Score: {question.questionScore.ToString()}";
        playerScoreText.text = $"Player Score: {quizManager.player.playerScore.ToString()}";
        answered = false;
    }

    IEnumerator PlayAudio()
    {
        if (question.questionType == QuestionType.Audio)
        {
            questionAudio.PlayOneShot(question.questionAudioClip);

            yield return new WaitForSeconds(question.questionAudioClip.length + .5f);
            StartCoroutine(PlayAudio());
        }
        else
        {
            StopCoroutine(PlayAudio());
            yield return null;
        }
    }

    void ImageHolder()
    {
        questionImage.transform.parent.gameObject.SetActive(true);
        questionImage.gameObject.SetActive(false);
        questionAudio.gameObject.SetActive(false);
        questionVideo.gameObject.SetActive(false);

    }

    void OnClick(Button btn)
    {
        if (!answered)
        {
            answered = true;
            bool val = quizManager.Answer(btn.name);

            if (val)
            {
                btn.image.color = correctCol;
            }
            else
            {
                btn.image.color = wrongCol;
            }
        }
    }

}
