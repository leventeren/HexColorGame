using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace vertigoGames.hexColorGame
{
    public class HexGrid : MonoBehaviour
    {
        private int m_width;
        private int m_height;
        private Canvas m_gridCanvas;
        public HexCell[] m_cells;
        private HexMesh m_hexMesh;
        [Header("Cell Game Object")]
        public HexCell cellPrefab;
        [Header("Cell Labels")]
        public Text cellLabelPrefab;
        public Text cellIndexLabelPrefab;
        internal int _upDown = 1;
        internal float _posz;

        int aColorIndex = 0;
        int bColorIndex = 0;
        int cColorIndex = 0;
        int dColorIndex = 0;
        [Header("Selected Cells")]
        public HexCell selectedCell1;
        public HexCell selectedCell2;
        public HexCell selectedCell3;

        bool animisruning = false;
        int _rotateCount = 3;
        public bool pointAdded = false;

        // Singleton
        private static HexGrid instance;
        // Construct
        private HexGrid() { }
        // Instance
        public static HexGrid Instance
        {
            get
            {
                if (instance == null)
                    instance = GameObject.FindObjectOfType(typeof(HexGrid)) as HexGrid;
                return instance;
            }
        }

        /// <summary>
        /// Oyun boardı oluşturur
        /// </summary>
        public void makeBoard()
        {
            m_width = GameManager.Instance.boardWidth;
            m_height = GameManager.Instance.boardHeight;
            m_gridCanvas = GetComponentInChildren<Canvas>();
            m_hexMesh = GetComponentInChildren<HexMesh>();
            m_cells = new HexCell[m_height * m_width];

            for (int z = 0, i = 0; z < m_height; z++)
            {
                for (int x = 0; x < m_width; x++)
                {
                    //tek sayı genişlik kontrolü?
                    if (m_width % 2 != 0)
                    {

                        //satır sonu?
                        if (x == m_width - 1)
                        {
                            //Debug.Log("Son satır (tek): " + _upDown);
                            CreateCell(x, z, i++, false);
                        }
                        else
                        {
                            if (_upDown % 2 == 0)
                            {
                                CreateCell(x, z, i++, true);
                                _upDown--;
                            }
                            else
                            {
                                CreateCell(x, z, i++, false);
                                _upDown++;
                            }
                        }
                        //satır sonu?
                    }
                    else
                    {
                        if (_upDown % 2 == 0)
                        {
                            CreateCell(x, z, i++, true);
                            _upDown--;
                        }
                        else
                        {
                            CreateCell(x, z, i++, false);
                            _upDown++;
                        }
                    }
                    //Debug.Log("CreateCell(" + x + "," + z + "," + i + " [" + _upDown + "])");                
                }
            }
            Debug.Log("Oyun bord(" + m_width + "x" + m_height + ") : OK");
        }

        /// <summary>
        /// Oyun hücrelerinin komşu değerleri
        /// </summary>
        public void neighborsMake()
        {
            //komşuları belirleyelim
            for (int z = 0, i = 0; z < m_height; z++)
            {
                for (int x = 0; x < m_width; x++)
                {
                    //açıklama satırları 8x4 boyut için
                    //Debug.Log(m_cells[i]);
                    if (i == 0)
                    {
                        m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + m_width], 3);
                        m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + 1 + m_width], 4);
                        m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + 1], 5);
                        m_cells[i].cellNeighborSet(m_cells[i], true);
                    }
                    else
                    {
                        //0.satır 1 2 3 4 5 6 7
                        if (i < m_width)
                        {
                            //0.satır tek sayılar 1 3 5 7
                            if (i % 2 != 0 || i == m_width - 1)
                            {
                                //0.satır son kare? (7)
                                if (i == m_width - 1)
                                {
                                    //0.satır sonra kare (çift)
                                    if (i % 2 == 0)
                                    {
                                        m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - 1], 1);
                                        m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - 1 + m_width], 2);
                                        m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + m_width], 3);
                                        m_cells[i].cellNeighborSet(m_cells[i], true);
                                    }
                                    //0.satır sonra kare (tek)
                                    else
                                    {
                                        m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - 1], 2);
                                        m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + m_width], 3);
                                        m_cells[i].cellNeighborSet(m_cells[i], true);
                                    }
                                }
                                //0.satır tek sayılar 1 3 5 (üçgen şeklinde seç) 1->0,9,2 / 3->2,11,4 / 5->4,13,6
                                else
                                {
                                    m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - 1], 2);
                                    m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + m_width], 3);
                                    m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + 1], 4);
                                    m_cells[i].cellNeighborSet(m_cells[i], true);
                                }
                            }
                            //0.satır çift sayılar 2 4 6
                            else
                            {
                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - 1], 1);
                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[(i - 1) + m_width], 2);
                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + m_width], 3);
                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + 1 + m_width], 4);
                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + 1], 5);
                                m_cells[i].cellNeighborSet(m_cells[i], true);
                            }
                        }
                        else
                        {
                            //en üst son satır [-24] 25 26 27 28 29 30 [-31]
                            if (i >= ((m_width * m_height) - m_width))
                            {
                                //en üst ilk ve son satırlar 24 31
                                if (i % m_width == 0)
                                {
                                    //ilk kare 24
                                    m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - m_width], 0);
                                    m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + 1], 5);
                                    m_cells[i].cellNeighborSet(m_cells[i], true);

                                    //son kare 31
                                    int _mcellii = i + m_width - 1;
                                    //çift (updown=down)
                                    if (m_width % 2 == 0)
                                    {
                                        m_cells[_mcellii].setNeighbors(0, 0, 0, true, m_cells[_mcellii], m_cells[_mcellii - m_width], 0);
                                        m_cells[_mcellii].setNeighbors(0, 0, 0, true, m_cells[_mcellii], m_cells[_mcellii - 1 - m_width], 1);
                                        m_cells[_mcellii].setNeighbors(0, 0, 0, true, m_cells[_mcellii], m_cells[_mcellii - 1], 2);
                                        m_cells[_mcellii].cellNeighborSet(m_cells[_mcellii], true);
                                    }
                                    //tek (updown=up)
                                    else
                                    {
                                        m_cells[_mcellii].setNeighbors(0, 0, 0, true, m_cells[_mcellii], m_cells[_mcellii - m_width], 0);
                                        m_cells[_mcellii].setNeighbors(0, 0, 0, true, m_cells[_mcellii], m_cells[_mcellii - 1], 1);
                                        m_cells[_mcellii].cellNeighborSet(m_cells[_mcellii], true);
                                    }
                                }
                                //25 26 27 28 29 30 [-31]
                                else
                                {
                                    if (!m_cells[i].checkCellNeighbor(m_cells[i]))
                                    {                                        
                                        //çift (updown=up) 26 28 30
                                        if (i % 2 == 0)
                                        {
                                            if (m_cells[i].downCell)
                                            {
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - m_width], 0);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - 1 - m_width], 1);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - 1], 2);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + 1], 4);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + 1 - m_width], 5);
                                            }
                                            else
                                            {
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - m_width], 0);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - 1], 1);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + 1], 5);
                                            }
                                            m_cells[i].cellNeighborSet(m_cells[i], true);
                                        }
                                        //tek (updown=down) 27 29
                                        else
                                        {                                            
                                            if (m_cells[i].downCell)
                                            {                                                
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - m_width], 0);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - 1 -m_width], 1);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - 1], 2);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + 1], 4);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + 1 - m_width], 5);
                                            }
                                            else
                                            {                                                
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - m_width], 0);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - 1], 1);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + 1], 5);
                                            }
                                            m_cells[i].cellNeighborSet(m_cells[i], true);
                                        }
                                    }
                                }
                            }
                            //1.satır 8 9 10 11 12 13 14 15 | 16 17 18 19 20 21 22 23
                            else
                            {                                
                                //1.satır ilk kare(ler) (8 16 24 ...)
                                if (i % m_width == 0)
                                {                                    
                                    m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - m_width], 0);
                                    m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + m_width], 3);
                                    m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + 1 + m_width], 4);
                                    m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + 1], 5);
                                    m_cells[i].cellNeighborSet(m_cells[i], true);

                                    //1.2. ve son satır son kare(ler) (15 23 31 ...)
                                    int _mcelli = i + m_width - 1;
                                    //çift (updown=down)
                                    if (m_width % 2 == 0)
                                    {
                                        m_cells[_mcelli].setNeighbors(0, 0, 0, true, m_cells[_mcelli], m_cells[_mcelli - m_width], 0);
                                        m_cells[_mcelli].setNeighbors(0, 0, 0, true, m_cells[_mcelli], m_cells[_mcelli - 1 - m_width], 1);
                                        m_cells[_mcelli].setNeighbors(0, 0, 0, true, m_cells[_mcelli], m_cells[_mcelli - 1], 2);
                                        m_cells[_mcelli].setNeighbors(0, 0, 0, true, m_cells[_mcelli], m_cells[_mcelli + m_width], 3);
                                        m_cells[_mcelli].cellNeighborSet(m_cells[_mcelli], true);
                                    }
                                    //tek (updown=up)
                                    else
                                    {
                                        m_cells[_mcelli].setNeighbors(0, 0, 0, true, m_cells[_mcelli], m_cells[_mcelli - m_width], 0);
                                        m_cells[_mcelli].setNeighbors(0, 0, 0, true, m_cells[_mcelli], m_cells[_mcelli - 1], 1);
                                        m_cells[_mcelli].setNeighbors(0, 0, 0, true, m_cells[_mcelli], m_cells[_mcelli - 1 + m_width], 2);
                                        m_cells[_mcelli].setNeighbors(0, 0, 0, true, m_cells[_mcelli], m_cells[_mcelli + m_width], 3);
                                        m_cells[_mcelli].cellNeighborSet(m_cells[_mcelli], true);
                                    }                                    
                                }                                
                                //1.satır 9 10 11 12 13 14 [-15] | 16 17 18 19 20 21 22 [-23]
                                else
                                {                                    
                                    //15 ve 23 hariç
                                    if (!m_cells[i].checkCellNeighbor(m_cells[i])) {
                                        
                                        //çift
                                        if (i % 2 == 0)
                                        {
                                            //upcell&downcell kontrolü
                                            //bord genişliğine göre hücreler yukarıda ya da aşağıda 
                                            //konumlandıkları için komşu pozisyonları değişiklik gösteriyor
                                            if (m_cells[i].downCell)
                                            {
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - m_width], 0);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - 1 - m_width], 1);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - 1], 2);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + m_width], 3);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + 1], 4);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - m_width + 1], 5);
                                                m_cells[i].cellNeighborSet(m_cells[i], true);
                                            }
                                            else
                                            {
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - m_width], 0);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - 1], 1);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - 1 + m_width], 2);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + m_width], 3);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + 1 + m_width], 4);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + 1], 5);
                                                m_cells[i].cellNeighborSet(m_cells[i], true);
                                            }
                                            
                                        }
                                        //tek
                                        else
                                        {
                                            //Debug.Log("heyo! " + m_cells[i].name);
                                            //Debug.Log("upcell");
                                            //upcell&downcell kontrolü
                                            //bord genişliğine göre hücreler yukarıda ya da aşağıda 
                                            //konumlandıkları için komşu pozisyonları değişiklik gösteriyor
                                            if (m_cells[i].downCell)
                                            {
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - m_width], 0);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - 1 - m_width], 1);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - 1], 2);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + m_width], 3);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + 1], 4);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + 1 - m_width], 5);
                                                m_cells[i].cellNeighborSet(m_cells[i], true);
                                            }
                                            else
                                            {
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - m_width], 0);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - 1], 1);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i - 1 + m_width], 2);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + m_width], 3);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + 1 + m_width], 4);
                                                m_cells[i].setNeighbors(0, 0, 0, true, m_cells[i], m_cells[i + 1], 5);
                                                m_cells[i].cellNeighborSet(m_cells[i], true);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //Debug.Log(m_cells[i].name);
                    i++;
                }
            }
            //komşuları belirleyelim
            //cell.SetNeighbors(x, z, i, downCell, m_cells[i],0);
            Debug.Log("Komşu değerlemesi: OK");
        }

        private void Start()
        {
            /* Hexagon mesh kullanımı */
            if (GameManager.Instance.hexagonMeshUse) { m_hexMesh.Triangulate(m_cells); }
        }

        /// <summary>
        /// Oyun boardı hücreleri oluşturuluyor
        /// <param name="x">x düzlemi.</param>
        /// <param name="z">z düzlemi.</param>
        /// <param name="i">index değeri.</param>
        /// <param name="downCell">Hücretin aşağıda/yukarıda olduğu bilgisi.</param>        
        /// </summary>
        private void CreateCell(int x, int z, int i, bool downCell = false)
        {
            if (downCell)
            {
                _posz = (HexMetrics.hexDrawFactor * (z + 1)) - (HexMetrics.hexDrawFactor / 2f);
            }
            else
            {
                _posz = HexMetrics.hexDrawFactor * (z + 1);
            }
            Vector3 position;
            position.x = x * (HexMetrics.hexDrawFactor - 2f);
            position.y = 0f;
            position.z = _posz;

            HexCell cell = m_cells[i] = Instantiate(cellPrefab);
            cell.name = "VertigoHexCell-" + i + "-" + HexCoordinates.FromOffsetCoordinates(x, z);
            cell.transform.SetParent(transform, false);
            cell.transform.localPosition = position;
            cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);

            /* Renk atamaları */
            int rColor = Random.Range(0, GameManager.Instance.colorPoints.Length);
            cell.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[rColor].color;
            cell.colorIndex = rColor;
            cell.hexCellIndex = i;
            cell.colorPoint = GameManager.Instance.colorPoints[rColor].point;
            cell.downCell = downCell;

            if (GameManager.Instance.debugMode)
            {
                Text label = Instantiate(cellLabelPrefab);
                label.rectTransform.SetParent(m_gridCanvas.transform, false);
                label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
                label.text = cell.coordinates.ToStringOnSeparateLines();

                Text labelIndex = Instantiate(cellIndexLabelPrefab);
                labelIndex.rectTransform.SetParent(m_gridCanvas.transform, false);
                labelIndex.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
                labelIndex.text = i.ToString();
            }
        }
        
        /// <summary>
        /// Random Dizi
        /// </summary>        
        public static void RandomizeArray<T>(T[] array)
        {
            int size = array.Length;
            for (int i = 0; i < size; i++)
            {
                int SwapIndex = Random.Range(i, size);
                T swapValue = array[i];
                array[i] = array[SwapIndex];
                array[SwapIndex] = swapValue;
            }
        }

        /// <summary>
        /// Hamle sayısı +1
        /// </summary>
        public void addMove()
        {
            GameManager.Instance.moves += 1;
            if (GameManager.Instance.movesText)
            {
                GameManager.Instance.movesText.GetComponent<Text>().text = GameManager.Instance.moves.ToString();
                if (GameManager.Instance.movesText.GetComponent<Animation>()) GameManager.Instance.movesText.GetComponent<Animation>().Play();
            }
        }

        /// <summary>
        /// Puanı kaydeder ve gösterir
        /// </summary>
        /// <param name="colorIndex">Renk index değeri</param>
        /// <param name="type">Toplam puanın çarpılacağı değer (5x3->3lü hücre 15puan - 5x4->4lü hücre 20 puan - 5x5->5li hücre 25 puan)</param>
        public void addPoint(int colorIndex, int type)
        {
            Debug.Log("point!");
            int newPoint = GameManager.Instance.colorPoints[colorIndex].point * type;
            GameManager.Instance.score += newPoint;

            //skor göstergesi güncelleme
            if (GameManager.Instance.scoreText)
            {
                GameManager.Instance.scoreText.GetComponent<Text>().text = GameManager.Instance.score.ToString();
                if (GameManager.Instance.scoreText.GetComponent<Animation>()) GameManager.Instance.scoreText.GetComponent<Animation>().Play();
            }
        }

        /// <summary>
        /// iTween animasyon hash oncomplete değeri
        /// </summary>
        void animationEnd()
        {
            Debug.Log("anim end!");
            animisruning = false;            
        }

        /// <summary>
        /// Komşu kontrolü sonrası verilen hücrelerin renk değişimi
        /// Ek :  puanlama işlemi, hamle sayısı değişimi
        /// </summary>
        /// <param name="cell1">1.hücre</param>
        /// <param name="cell2">2.hücre</param>
        /// <param name="cell3">3.hücre</param>
        /// <param name="cell4">4.hücre</param>
        /// <param name="cell5">5.hücre</param>
        /// <param name="type">Puanlanacak hücre miktarı</param>
        /// <param name="start">Oyun başlangıcında kontrol</param>
        public void cellColorChange(HexCell cell1,HexCell cell2, HexCell cell3, HexCell cell4=null, HexCell cell5=null, int type=0, bool start = false)
        {
            if (start == false)
            {
                //puanlama
                addPoint(cell1.colorIndex, type);
                addMove();

                //Animasyonlar
                if (!animisruning)
                {
                    animisruning = true;
                    //iTween.ScaleFrom(cell1.gameObject, iTween.Hash("scale", new Vector3(0.5f, 0.5f, 0.5f), "speed", 5f, "oncomplete", "animationEnd"));
                    //StartCoroutine(DoWaitTest());
                    //iTween.ScaleFrom(cell2.gameObject, iTween.Hash("scale", new Vector3(0.5f, 0.5f, 0.5f), "speed", 5f, "oncomplete", "animationEnd"));
                    //StartCoroutine(DoWaitTest());
                    //iTween.ScaleFrom(cell3.gameObject, iTween.Hash("scale", new Vector3(0.5f, 0.5f, 0.5f), "speed", 5f, "oncomplete", "animationEnd"));
                }

                Instantiate(GameManager.Instance.cellExplosionEffect, cell1.transform.position, cell1.transform.rotation);
                Instantiate(GameManager.Instance.cellExplosionEffect, cell2.transform.position, cell2.transform.rotation);
                Instantiate(GameManager.Instance.cellExplosionEffect, cell3.transform.position, cell3.transform.rotation);

            }

            //renk tanımlamalarından random uniq değerler seçelim
            int[] colorArray = new int[GameManager.Instance.colorPoints.Length];
            for (int i = 0; i < colorArray.Length; ++i)
            {
                colorArray[i] = i;
                RandomizeArray<int>(colorArray);
            }
            int value0 = Random.Range(0, GameManager.Instance.colorPoints.Length);
            int value1 = Random.Range(0, GameManager.Instance.colorPoints.Length);
            int value2 = Random.Range(0, GameManager.Instance.colorPoints.Length);
            int value3 = Random.Range(0, GameManager.Instance.colorPoints.Length);
            int value4 = Random.Range(0, GameManager.Instance.colorPoints.Length);

            //Debug.Log(colorArray[0] + " " + colorArray[1] + " " + colorArray[2] + " " + colorArray[3]);
            //Debug.Log(value0 + " " + value1 + " " + value2 + " " + value3);

            cell1.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[colorArray[value0]].color;
            cell1.colorIndex = colorArray[value0];            

            cell2.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[colorArray[value1]].color;
            cell2.colorIndex = colorArray[value1];            

            cell3.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[colorArray[value2]].color;
            cell3.colorIndex = colorArray[value2];
            
            if (cell4!=null)
            {
                cell4.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[colorArray[value3]].color;
                cell4.colorIndex = colorArray[value3];
            }
            if (cell5!=null)
            {
                cell5.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[colorArray[value4]].color;
                cell5.colorIndex = colorArray[value4];
            }
            
        }

        /// <summary>
        /// Verilen 4 hücre renk değerlerinin birbirine eşit olup olmadığının kontrolü.
        /// 3 Hücre kontrolü için son değer -1 olmalı
        /// </summary>
        /// <param name="a">Renk index 1</param>
        /// <param name="b">Renk index 2</param>
        /// <param name="c">Renk index 3</param>
        /// <param name="d">Renk index 4</param>
        /// <returns>True/False</returns>
        public bool colorIndexChecker(int a,int b,int c,int d=-1)
        {
            if (d!=-1)
            {
                if ((a == b) && (a == c) && (a == d) && (b == d))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if ((a == b) && (a == c) && (b == c))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }                
        }

        /// <summary>
        /// Tüm board puanlama için kontrol edilir
        /// </summary>
        /// <param name="start">Oyun başlangıç kontrolü (puan vermeden)</param>
        public bool checkCellPoint(bool start = false)
        {
            bool changed = false;
            Debug.Log("Check point start!");
            for (int z = 0, i = 0; z < m_height; z++)
            {
                for (int x = 0; x < m_width; x++)
                {
                    //Debug.Log(m_cells[i].name);
                    //seçili hücrenin renk index değeri [0 1 2 3 4 5]
                    aColorIndex = m_cells[i].colorIndex;

                    //kendisi+2+3
                    if ((m_cells[i].hexKomsu2 && m_cells[i].hexKomsu2.colorIndex == aColorIndex) && (m_cells[i].hexKomsu3 && m_cells[i].hexKomsu3.colorIndex == aColorIndex))
                    {
                        //?2->3
                        if (m_cells[i].hexKomsu2.hexKomsu3 && m_cells[i].hexKomsu2.hexKomsu3.colorIndex == aColorIndex)
                        {

                            if (m_cells[i].hexKomsu4 && m_cells[i].hexKomsu4.colorIndex == aColorIndex)
                            {
                                //kendisi+2+3+(2->3)+4
                                Debug.Log("PUANN! 5hücre " + m_cells[i].name + " " + m_cells[i].hexKomsu2.name + " " + m_cells[i].hexKomsu3.name + " " + m_cells[i].hexKomsu2.hexKomsu3.name + " " + m_cells[i].hexKomsu4.name);
                                cellColorChange(m_cells[i], m_cells[i].hexKomsu2, m_cells[i].hexKomsu3, m_cells[i].hexKomsu2.hexKomsu3, m_cells[i].hexKomsu4, 5, start);
                                changed = true;
                                pointAdded = true;
                            }
                            else
                            {
                                //kendisi+2+3+(2->3)
                                Debug.Log("PUANN! 4hücre " + m_cells[i].name + " " + m_cells[i].hexKomsu2.name + " " + m_cells[i].hexKomsu3.name + " " + m_cells[i].hexKomsu2.hexKomsu3.name);
                                cellColorChange(m_cells[i], m_cells[i].hexKomsu2, m_cells[i].hexKomsu3, m_cells[i].hexKomsu2.hexKomsu3,null, 4, start);
                                changed = true;
                                pointAdded = true;
                            }
                        }                        
                        else
                        {
                            //kendisi+2+3
                            Debug.Log("PUANN! 3hücre " + m_cells[i].name + " " + m_cells[i].hexKomsu2.name + " " + m_cells[i].hexKomsu3.name);
                            cellColorChange(m_cells[i], m_cells[i].hexKomsu2, m_cells[i].hexKomsu3, null, null, 3, start);
                            changed = true;
                            pointAdded = true;
                        }
                    }
                    else if ((m_cells[i].hexKomsu3 && m_cells[i].hexKomsu3.colorIndex == aColorIndex) && (m_cells[i].hexKomsu4 && m_cells[i].hexKomsu4.colorIndex == aColorIndex))
                    {
                        //?3->4
                        if (m_cells[i].hexKomsu3.hexKomsu4 && m_cells[i].hexKomsu3.hexKomsu4.colorIndex == aColorIndex)
                        {

                            if (m_cells[i].hexKomsu2 && m_cells[i].hexKomsu2.colorIndex == aColorIndex)
                            {
                                //kendisi+3+4+(3->4)+2
                                Debug.Log("PUANN! 5hücre " + m_cells[i].name + " " + m_cells[i].hexKomsu3.name + " " + m_cells[i].hexKomsu4.name + " " + m_cells[i].hexKomsu3.hexKomsu4.name + " " + m_cells[i].hexKomsu2.name);
                                cellColorChange(m_cells[i], m_cells[i].hexKomsu3, m_cells[i].hexKomsu4, m_cells[i].hexKomsu3.hexKomsu4, m_cells[i].hexKomsu2, 5, start);
                                changed = true;
                                pointAdded = true;
                            }
                            else
                            {
                                //kendisi+3+4+(3->4)
                                Debug.Log("PUANN! 4hücre " + m_cells[i].name + " " + m_cells[i].hexKomsu3.name + " " + m_cells[i].hexKomsu4.name + " " + m_cells[i].hexKomsu3.hexKomsu4.name);
                                cellColorChange(m_cells[i], m_cells[i].hexKomsu3, m_cells[i].hexKomsu4, m_cells[i].hexKomsu3.hexKomsu4, null, 4, start);
                                changed = true;
                                pointAdded = true;
                            }
                        }
                        else
                        {
                            //kendisi+2+3
                            Debug.Log("PUANN! 3hücre " + m_cells[i].name + " " + m_cells[i].hexKomsu3.name + " " + m_cells[i].hexKomsu4.name);
                            cellColorChange(m_cells[i], m_cells[i].hexKomsu3, m_cells[i].hexKomsu4, null, null, 3, start);
                            changed = true;
                            pointAdded = true;
                        }
                    }
                    i++;
                }
            }
            //eğer puan sonrasında hücrelerde tekrar puan alacak konum varsa yeniden çalıştır
            if (changed) checkCellPoint(true);
            return changed;
        }


        /// <summary>
        /// Seçili hücrelerin renklerini birbirine transfer eder (çevirir)
        /// </summary>
        /// <param name="cell1">Hücre 1</param>
        /// <param name="cell2">Hücre 2</param>
        /// <param name="cell3">Hücre 3</param>
        /// <param name="directionDown">Transfer yönü</param>
        public void swapCell(HexCell cell1, HexCell cell2, HexCell cell3, bool directionDown = true)
        {
            int _cell1ColorIndex = cell1.colorIndex;
            int _cell2ColorIndex = cell2.colorIndex;
            int _cell3ColorIndex = cell3.colorIndex;

            //Debug.Log(cell1.name + "[" + _cell1ColorIndex + "] " + cell2.name + "[" + _cell2ColorIndex + "] " + cell3.name + "[" + _cell3ColorIndex + "]");

            /*
            if (directionDown)
            {
                cell1.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[_cell3ColorIndex].color;
                cell1.colorIndex = _cell3ColorIndex;
                cell2.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[_cell1ColorIndex].color;
                cell2.colorIndex = _cell1ColorIndex;
                cell3.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[_cell2ColorIndex].color;
                cell3.colorIndex = _cell2ColorIndex;
            } else
            {
                cell1.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[_cell2ColorIndex].color;
                cell1.colorIndex = _cell2ColorIndex;
                cell2.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[_cell3ColorIndex].color;
                cell2.colorIndex = _cell3ColorIndex;
                cell3.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[_cell1ColorIndex].color;
                cell3.colorIndex = _cell1ColorIndex;
            }
            checkCellPoint(false);
            */

            //seçili hücrenin renklerini birbirine transfer edelim çevirelim
            StartCoroutine(rotateCellEffect(cell1, cell2, cell3, directionDown));
            //çevirme işlemi sonrasında puan eklenip eklenmediği kontrolü için false
            pointAdded = false;
            
            //Debug.Log("SWAP END!");
        }

        /// <summary>
        /// Seçili hücreleri sürüklenen yöne göre döndürür
        /// </summary>
        /// <param name="cell1">Hücre 1</param>
        /// <param name="cell2">Hücre 2</param>
        /// <param name="cell3">Hücre 3</param>
        /// <param name="directionDown">Yön</param>
        /// <returns></returns>
        IEnumerator rotateCellEffect(HexCell cell1, HexCell cell2, HexCell cell3, bool directionDown = true)
        {
            int _cell1ColorIndex;
            int _cell2ColorIndex;
            int _cell3ColorIndex;

            _cell1ColorIndex = cell1.colorIndex;
            _cell2ColorIndex = cell2.colorIndex;
            _cell3ColorIndex = cell3.colorIndex;

            if (directionDown)
            {
                cell1.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[_cell3ColorIndex].color;
                cell1.colorIndex = _cell3ColorIndex;
                cell2.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[_cell1ColorIndex].color;
                cell2.colorIndex = _cell1ColorIndex;
                cell3.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[_cell2ColorIndex].color;
                cell3.colorIndex = _cell2ColorIndex;

                checkCellPoint(false);
                if (pointAdded==true) { StopAllCoroutines(); }

                //Debug.Log("1");
                yield return new WaitForSeconds(.3f);

                cell1.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[_cell2ColorIndex].color;
                cell1.colorIndex = _cell2ColorIndex;
                cell2.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[_cell3ColorIndex].color;
                cell2.colorIndex = _cell3ColorIndex;
                cell3.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[_cell1ColorIndex].color;
                cell3.colorIndex = _cell1ColorIndex;

                checkCellPoint(false);
                if (pointAdded == true) { StopAllCoroutines(); }

                //Debug.Log("2");
                yield return new WaitForSeconds(.3f);

                cell1.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[_cell1ColorIndex].color;
                cell1.colorIndex = _cell1ColorIndex;
                cell2.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[_cell2ColorIndex].color;
                cell2.colorIndex = _cell2ColorIndex;
                cell3.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[_cell3ColorIndex].color;
                cell3.colorIndex = _cell3ColorIndex;

                checkCellPoint(false);
                if (pointAdded == true) { StopAllCoroutines(); }

                //Debug.Log("3");
                yield return new WaitForSeconds(.3f);

            }
            else
            {
                cell1.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[_cell2ColorIndex].color;
                cell1.colorIndex = _cell2ColorIndex;
                cell2.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[_cell3ColorIndex].color;
                cell2.colorIndex = _cell3ColorIndex;
                cell3.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[_cell1ColorIndex].color;
                cell3.colorIndex = _cell1ColorIndex;

                checkCellPoint(false);
                if (pointAdded == true) { StopAllCoroutines(); }

                //Debug.Log("1");
                yield return new WaitForSeconds(.3f);

                cell1.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[_cell3ColorIndex].color;
                cell1.colorIndex = _cell3ColorIndex;
                cell2.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[_cell1ColorIndex].color;
                cell2.colorIndex = _cell1ColorIndex;
                cell3.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[_cell2ColorIndex].color;
                cell3.colorIndex = _cell2ColorIndex;

                checkCellPoint(false);
                if (pointAdded == true) { StopAllCoroutines(); }

                //Debug.Log("1");
                yield return new WaitForSeconds(.3f);

                cell1.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[_cell1ColorIndex].color;
                cell1.colorIndex = _cell1ColorIndex;
                cell2.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[_cell2ColorIndex].color;
                cell2.colorIndex = _cell2ColorIndex;
                cell3.hexSpriteObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.colorPoints[_cell3ColorIndex].color;
                cell3.colorIndex = _cell3ColorIndex;

                checkCellPoint(false);
                if (pointAdded == true) { StopAllCoroutines(); }

                //Debug.Log("3");
                yield return new WaitForSeconds(.3f);

            }           

        }

    }
}