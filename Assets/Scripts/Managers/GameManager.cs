using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Get
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameManager>();
            return instance;
        }
    }

    private void Awake()
    {
        lifes = new List<RectTransform>();
        #region 내가 직접 짠거
        //float a = 0;
        //// 최초 생성될 라이프 개수
        //int length = 10;
        //for (int i = 0; i < length; ++i)
        //{
        //    RectTransform rt = Instantiate(lifePrefab, lifeTransform).GetComponent<RectTransform>();
        //    rt.localPosition = new Vector2(0, a);
        //    RectTransform rt2 = Instantiate(lifePrefab, lifeTransform).GetComponent<RectTransform>();
        //    rt2.localPosition = new Vector2(33.2f, a);
        //    a -= 33.2f;
        //}
# endregion
        int length = 5;
        for (int i = 0; i < length; ++i)
        {
            Vector2 vec = new Vector2(0, -(i * 33.2f));
            // i = 0 ? Vector2(0,0)
            // i = 1 ? Vector2(0, -33.2f)

            RectTransform rt = Instantiate(lifePrefab, lifeTransform).GetComponent<RectTransform>();
            rt.localPosition = vec;
            lifes.Add(rt);

            vec.x = 33.2f;

            rt = Instantiate(lifePrefab, lifeTransform).GetComponent<RectTransform>();
            rt.localPosition = vec;
            lifes.Add(rt);
            
        }
        Money = 100;
        #region 간결화
        //for (int i = 0; i < 10; ++i)
        //{
        //    RectTransform rt = Instantiate(lifePrefab, lifeTransform).GetComponent<RectTransform>();
        //
        //    //삼항연산자 (조건문 ? true : false
        //    // i == 0 ? 1 : 0
        //    //i 값이 0과 동일한가? 맞으면 1반환, 아니면 0 반환
        //    rt.localPosition = new Vector2(i % 2 == 0 ? 0 : 32, -(i * 33.2f));
        //}
        #endregion
    }
    public RectTransform lifeTransform = null;
    public GameObject lifePrefab = null;
    public Text moneyText = null;

    public GameObject PopUp = null;
    public Text PrevRoundText = null;
    public Text CurrentRoundText = null;

    private int money;
    public int Money
    {
        get { return money; }
        set
        {
            money = value;
            moneyText.text = "<b>Money</b>\n" + money.ToString();
        }
    }

    private List<RectTransform> lifes = null;
    public void AddLife()
    {
        if (lifes.Count == 0) return;
        Vector3 vec = lifes[lifes.Count - 1].localPosition;
        RectTransform rt = Instantiate(lifePrefab, lifeTransform).GetComponent<RectTransform>();
        if(vec.x == 33.2f)
        {
            rt.localPosition = new Vector2(0, vec.y - 33.2f);
        }
        else
        {
            rt.localPosition = new Vector2(33.2f, vec.y);
        }

            lifes.Add(rt);

    }

    public bool isGameOver = false;
    public void RemoveLife()
    {
        if (isGameOver) return;

        //List안에 life가 두개 들어있다고 가정
        // 첫 번째 라이프는 lifes[0] 에 들어 있다, ex)0, 1
        // Count 는 진짜 개수를 반환하기 때문에 ,lifes.Count == 2를 반환
        //그래서 리스트의 마지막 원소를 접근하기 위해서는 lifes.Count - 1 로 접근이 가능!
        RectTransform value = lifes[lifes.Count - 1];
        //인덱스로 삭제하는 방법
        lifes.RemoveAt(lifes.Count - 1);
        Destroy(value.gameObject);

        if(lifes.Count == 0)
        {
            PopUp.SetActive(true);
            int currentRound = SpawnManager.Get.roundCount;

            //저장되어있는 값이 있으면 그값을 주고 
            //없으면 디폴트 값을 준다 int = 0
            int bestRound = PlayerPrefs.GetInt("BestRound");

            if(currentRound > bestRound)
            {
                PlayerPrefs.SetInt("BestRound", currentRound);
                bestRound = currentRound;
            }

            CurrentRoundText.text = currentRound.ToString();
            PrevRoundText.text = bestRound.ToString();

            Time.timeScale = 0;
            isGameOver = true;
            return;
        }

    }
    //decimal : 소수점 5칸만! 정확함 

    private bool isFoldLife = false;
    public void OnFoldLife()
    {
        //if(isFoldLife == true)

        //if (isFoldLife) isFoldLife = false;
        //else isFoldLife = true;

        isFoldLife = !isFoldLife;
        if(isFoldLife)
        {
            lifeTransform.localPosition = new Vector3(
                lifeTransform.localPosition.x,
                -375f);
        }
        else
        {
            lifeTransform.localPosition = new Vector3(
                lifeTransform.localPosition.x,
                0f);
        }
    }

    public void OnLoadTitle()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}