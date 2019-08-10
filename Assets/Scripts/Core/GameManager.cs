using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using vertigoGames.hexColorGame.Types;

/*
*****************************************************************
*** YARARLANDIĞIM KAYNAKLARI ************************************
Hexagonlar konusunda detaylı bilgi:
https://www.redblobgames.com/grids/hexagons/#basics
Hexagon koordinat/açı/kenar/alan hesaplamaları ve doğrulaması için görsel araç:
https://www.mathsisfun.com/geometry/hexagon.html
Hexagon oyunlarla ilgili tutorial:
https://gamedevelopment.tutsplus.com/tutorials/introduction-to-axial-coordinates-for-hexagonal-tile-based-games--cms-28820
*****************************************************************
* 
* Animasyonlar için 3rd party tool : https://assetstore.unity.com/packages/tools/animation/itween-84
* Dökümantasyon : http://www.pixelplacement.com/itween/documentation.php
* (çevirme ve patlama gibi animasyonlar zaman sebebi ile yetişmedi)
* 
* Komşu sisteminde dizilerde swap işlemi gerçekleştirildiğinde eski komşu değeri dizide null atanamıyor.
* Geçici aynı boyutta yeni bir dizi (_tmpCell = new HexCell[boardWidth * boardHeight];) ile swap yapılabiliyor
* fakat yeni kopyanın boyutu eskiye atıldığında yine benzer bir sorun oluşuyor.
* Çözüm için zaman olmadığından diziler yerine döngülerrrrrrrr ve iflerrrrr ile yapıldı.
* Diziler ile olan kodlarda kapalı ve başında ( Diziler ile; ) belirteci mevcut
* HexCell.cs (komşu belirleme/kontrol kodları) setNeighbors,OnPointerClick,selectCellAndNeighbors
*/

namespace vertigoGames.hexColorGame
{
    public class GameManager : MonoBehaviour
    {
        [Tooltip("Hexagon koordinat/index/komşu bilgileri gösterimi. Hexagonlar içerisinde gameobject kullanılıyorsa pasif yapın!")]
        public bool debugMode = false;
        [Space]
        [Header("Game Managers")]
        public HexGrid hexgrid;
        public GameObject gameMainCanvas;
        public GameObject gameOverCanvas;
        [Header("Board Settings")]
        [Range(4, 8)]
        [Tooltip("Oyun hex kutu genişlik ayarı")]
        public int boardWidth = 8;
        [Range(3, 9)]
        [Tooltip("Oyun hex kutu yükseklik ayarı")]
        public int boardHeight = 9;
        [Header("Mesh Settings")]
        [Tooltip("Hexagonlarda mesh/gameobject kullanımı")]
        public bool hexagonMeshUse = false;
        [Tooltip("Mesh kullanımında kaplama rengi (genel)")]
        public Color meshColor;
        [Tooltip("Hezagon hücrelerin derinlik değeri")]
        [Range(0, 5)]
        public float hexagonCellHeight = 2f;
        [Header("Moves")]
        public Text movesText;
        public int moves;
        [Space]
        [Header("Point Settings")]
        public int score = 0;
        internal int highScore = 0;
        public Text scoreText;
        public Text highScoreText;
        public colorPoints[] colorPoints;
        public string scorePlayerPrefs = "vertigoGamesHighScore";
        internal bool isGameOver = false;
        [Header("Impact Effect")]
        public GameObject cellExplosionEffect;
        [Header("_tmp cell for debug")]
        public HexCell[] _tmpCell;

        // Singleton
        private static GameManager instance;
        // Construct
        private GameManager() { }
        // Instance
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                    instance = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
                return instance;
            }
        }

        /// <summary>
        /// Board oluşumu, komşuların belirlenmesi ve kontrolü
        /// </summary>
        private void Awake()
        {
            //board oluştur
            HexGrid.Instance.makeBoard();
            //komşu değerleri belirle
            HexGrid.Instance.neighborsMake();
            //başlangıçta aynı renklerin yanyana gelme ihtimaline karşı
            HexGrid.Instance.checkCellPoint(true);
            HexGrid.Instance.pointAdded = false;
            //swap işlemleri için yeni bir array (gerekirse)
            _tmpCell = new HexCell[boardWidth * boardHeight];            
            
        }

        /// <summary>
        /// Skor bilgilerini alalım
        /// </summary>
        void Start()
        {
            /* skor bilgileri */
            highScore = PlayerPrefs.GetInt(scorePlayerPrefs, 0);
            if (highScore != 0)
            {
                highScoreText.text = "Highscore : " + highScore.ToString();
            } else
            {
                highScoreText.text = "Highscore : 0";
            }            
        }        

        /// <summary>
        /// Geri tuşuna basılırsa oyunu kapatalım
        /// </summary>
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            } else { return; }
        }

        /// <summary>
        /// Oyun bitimi skor güncellemesi
        /// </summary>
        void gameOver()
        {
            /* skor en yüksek skordan büyükse yeni skoru yazalım */
            highScoreCheck();
        }

        /// <summary>
        /// Uygulama kapanma kontrolü
        /// </summary>
        public void OnApplicationQuit()
        {
            /* oyun bitmeden çıkarsa skoru kaydedelim */
            highScoreCheck();
        }

        /// <summary>
        /// Yüksek skor kontrolü
        /// </summary>
        public void highScoreCheck()
        {
            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetInt(scorePlayerPrefs, score);
            }
        }

        /// <summary>
        /// S Butonu
        /// </summary>
        public void devReindexBoard()
        {
            Debug.Log("S");
            //HexGrid.Instance.neighborsMake();
            /*for(int i=0;i<=HexGrid.Instance.m_cells.Length-1;i++)
            {
                Debug.Log(HexGrid.Instance.m_cells[i].name);
            }*/

            /*
            _tmpCell[0] = HexGrid.Instance.m_cells[0];
            HexGrid.Instance.m_cells[0] = HexGrid.Instance.m_cells[1];
            HexGrid.Instance.m_cells[1] = _tmpCell[0];
            HexGrid.Instance.neighborsMake();
            */

            //GetComponent<Renderer>().material.color = initialColor;
            
            //visual feedback:
            //iTween.ColorFrom(HexGrid.Instance.m_cells[0].gameObject, new Color(1f, 1f, .1f), .3f);
            //iTween.ScaleFrom(HexGrid.Instance.m_cells[0].gameObject, new Vector3(0.5f, 0.5f, 0.5f), .5f);


            

            float travelTime = 10f;
            iTween.MoveTo(HexGrid.Instance.m_cells[0].gameObject, iTween.Hash("position", HexGrid.Instance.m_cells[3].gameObject.transform, "time", travelTime, "easetype", "linear", "oncomplete", "MoveToWaypoint", "Looktarget", HexGrid.Instance.m_cells[4].gameObject.transform, "looktime", .4));

        }

        /// <summary>
        /// R butonu
        /// </summary>
        public void devR()
        {
            Debug.Log("R");
            while (HexGrid.Instance.checkCellPoint(false))
            {
                HexGrid.Instance.checkCellPoint(false);
            }
        }        
    }
}