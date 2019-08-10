using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;

namespace vertigoGames.hexColorGame
{
    public class HexCell : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler, IEndDragHandler, IBeginDragHandler
    {
        internal HexCoordinates coordinates;
        [Header("Bilgi")]
        [Tooltip("Cell index bilgisi")]
        public int hexCellIndex = 0;
        [Tooltip("Cell atanan renk index bilgisi")]
        public int colorIndex;
        [Tooltip("Cell atanan renk puan bilgisi")]
        public int colorPoint;
        //drag kontrolü için
        [Tooltip("Cell seçim durumu")]
        public bool selected = false;
        public bool downCell = false;
        [Header("Komþu Hücreler (array)")]
        [SerializeField]
        HexCell[] hexNeighbors;        
        //public HexCell[] hexNeighborsForSize;
        [Header("Komþu deðerlemesi durumu")]
        public bool neighborSet = false;
        [Header("Hex Game Models")]
        public GameObject hexGameObject;
        public GameObject hexBorder;
        public GameObject hexHightlight;
        public GameObject hexSpriteObject;
        bool _checkValue = false;
        float _vposition1 = 0;
        float _vposition2 = 0;

        [Header("Komþu Hücreler")]
        public HexCell hexKomsu0;
        public HexCell hexKomsu1;
        public HexCell hexKomsu2;
        public HexCell hexKomsu3;
        public HexCell hexKomsu4;
        public HexCell hexKomsu5;

        // Singleton
        private static HexCell instance;
        // Construct
        private HexCell() { }
        // Instance
        public static HexCell Instance
        {
            get
            {
                if (instance == null)
                    instance = GameObject.FindObjectOfType(typeof(HexCell)) as HexCell;
                return instance;
            }
        }

        /// <summary>
        /// Komþu deðerlemesi kontrolü
        /// <param name="cell">Hücre</param>
        /// <returns>True/False</returns>
        /// </summary>
        public bool checkCellNeighbor(HexCell cell)
        {
            if (cell.neighborSet) { return true; } else { return false; }
        }

        /// <summary>
        /// Komþu deðerlemesi durum deðiþimi
        /// <param name="cell">Hücre</param>
        /// <param name="value">Deðer</param>
        /// </summary>
        public void cellNeighborSet(HexCell cell, bool value)
        {
            cell.neighborSet = value;
        }

        /// <summary>
        /// Komþu deðerlemesi
        /// <param name="x">x düzlemi</param>
        /// <param name="z">z düzlemi</param>
        /// <param name="i">index</param>
        /// <param name="_downCell">Hücre üstte/altta bilgisi</param>
        /// <param name="cell">Hücre</param>
        /// <param name="cellNeighbors">Komþu hücreler</param>
        /// <param name="hexNeighborIndex">Hücre index deðeri</param>
        /// </summary>
        public void setNeighbors(int x, int z, int i, bool _downCell, HexCell cell, HexCell cellNeighbors, int hexNeighborIndex)
        {
            //Debug.Log(x + "-" + z + "-" + i + "-" + _downCell + "- " + cell.name + " -> " + cellNeighbors.name + " - " + hexNeighborIndex);
            //cell.hexNeighbors[hexNeighborIndex] = cellNeighbors;

            switch (hexNeighborIndex)
            {
                case 0:
                    cell.hexKomsu0 = cellNeighbors;
                    break;
                case 1:
                    cell.hexKomsu1 = cellNeighbors;
                    break;
                case 2:
                    cell.hexKomsu2 = cellNeighbors;
                    break;
                case 3:
                    cell.hexKomsu3 = cellNeighbors;
                    break;
                case 4:
                    cell.hexKomsu4 = cellNeighbors;
                    break;
                case 5:
                    cell.hexKomsu5 = cellNeighbors;
                    break;
            }
            
        }

        /// <summary>
        /// Tüm komþu bilgilerini verir
        /// </summary>
        /// <param name="cell">Hücre</param>
        public List<HexCell> GetAllNeighbors(HexCell cell)
        {
            List<HexCell> neighborsObject = new List<HexCell>();

            //Debug.Log(cell.hexNeighbors[0].name);
            for (int i = 0; i <= hexNeighbors.Length - 1; i++)
            {
                if (cell.hexNeighbors[i]) {
                    //Debug.Log(cell.hexNeighbors[i].name);
                    neighborsObject.Add(cell.hexNeighbors[i]);                    
                }
            }
            return neighborsObject;
        }

        /// <summary>
        /// Hücre seçim event
        /// </summary>
        /// <param name="eventData">Pointer event data</param>
        public void OnPointerClick(PointerEventData eventData)
        {
            //seçilen hücreyi belirginleþtirelim
            iTween.ScaleFrom(this.gameObject, new Vector3(1.1f, 1.1f, 1.1f), .1f);
            HexGrid.Instance.selectedCell1 = this;
            //hücre seçiliyse tekrar basýldýðýnda random 2 komþu hücre seçilecek
            if (selected)
            {
                int _randomNeighbor = UnityEngine.Random.Range(0, 6);
                //Debug.Log("click! " + _randomNeighbor);
                if (_randomNeighbor == 0)
                {
                    if (hexKomsu0 && hexKomsu1)
                    {
                        clearAllSelectCell(this);
                        selectBorder();
                        hexKomsu0.selected = true;
                        hexKomsu0.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell2 = hexKomsu0;
                        hexKomsu1.selected = true;
                        hexKomsu1.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell3 = hexKomsu1;
                    }
                    else if (hexKomsu1 && hexKomsu2)
                    {
                        clearAllSelectCell(this);
                        selectBorder();
                        hexKomsu1.selected = true;
                        hexKomsu1.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell2 = hexKomsu1;
                        hexKomsu2.selected = true;
                        hexKomsu2.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell3 = hexKomsu2;
                    }
                    else if (hexKomsu2 && hexKomsu3)
                    {
                        clearAllSelectCell(this);
                        selectBorder();
                        hexKomsu2.selected = true;
                        hexKomsu2.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell2 = hexKomsu2;
                        hexKomsu3.selected = true;
                        hexKomsu3.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell3 = hexKomsu3;
                    }
                    else if (hexKomsu3 && hexKomsu4)
                    {
                        clearAllSelectCell(this);
                        selectBorder();
                        hexKomsu3.selected = true;
                        hexKomsu3.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell2 = hexKomsu3;
                        hexKomsu4.selected = true;
                        hexKomsu4.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell3 = hexKomsu4;
                    }
                    else if (hexKomsu4 && hexKomsu5)
                    {
                        clearAllSelectCell(this);
                        selectBorder();
                        hexKomsu4.selected = true;
                        hexKomsu4.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell2 = hexKomsu4;
                        hexKomsu5.selected = true;
                        hexKomsu5.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell3 = hexKomsu5;
                    }
                }
                else if (_randomNeighbor == 1)
                {
                    if (hexKomsu1 && hexKomsu2)
                    {
                        clearAllSelectCell(this);
                        selectBorder();
                        hexKomsu1.selected = true;
                        hexKomsu1.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell2 = hexKomsu1;
                        hexKomsu2.selected = true;
                        hexKomsu2.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell3 = hexKomsu2;
                    }
                    else if (hexKomsu2 && hexKomsu3)
                    {
                        clearAllSelectCell(this);
                        selectBorder();
                        hexKomsu2.selected = true;
                        hexKomsu2.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell2 = hexKomsu2;
                        hexKomsu3.selected = true;
                        hexKomsu3.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell3 = hexKomsu3;
                    }
                    else if (hexKomsu3 && hexKomsu4)
                    {
                        clearAllSelectCell(this);
                        selectBorder();
                        hexKomsu3.selected = true;
                        hexKomsu3.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell2 = hexKomsu3;
                        hexKomsu4.selected = true;
                        hexKomsu4.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell3 = hexKomsu4;
                    }
                    else if (hexKomsu4 && hexKomsu5)
                    {
                        clearAllSelectCell(this);
                        selectBorder();
                        hexKomsu4.selected = true;
                        hexKomsu4.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell2 = hexKomsu4;
                        hexKomsu5.selected = true;
                        hexKomsu5.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell3 = hexKomsu5;
                    }
                    else if (hexKomsu5 && hexKomsu0)
                    {
                        clearAllSelectCell(this);
                        selectBorder();
                        hexKomsu5.selected = true;
                        hexKomsu5.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell2 = hexKomsu5;
                        hexKomsu0.selected = true;
                        hexKomsu0.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell3 = hexKomsu0;
                    }
                }
                else if (_randomNeighbor == 2)
                {
                    if (hexKomsu2 && hexKomsu3)
                    {
                        clearAllSelectCell(this);
                        selectBorder();
                        hexKomsu2.selected = true;
                        hexKomsu2.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell2 = hexKomsu2;
                        hexKomsu3.selected = true;
                        hexKomsu3.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell3 = hexKomsu3;
                    }
                    else if (hexKomsu3 && hexKomsu4)
                    {
                        clearAllSelectCell(this);
                        selectBorder();
                        hexKomsu3.selected = true;
                        hexKomsu3.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell2 = hexKomsu3;
                        hexKomsu4.selected = true;
                        hexKomsu4.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell3 = hexKomsu4;
                    }
                    else if (hexKomsu4 && hexKomsu5)
                    {
                        clearAllSelectCell(this);
                        selectBorder();
                        hexKomsu4.selected = true;
                        hexKomsu4.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell2 = hexKomsu4;
                        hexKomsu5.selected = true;
                        hexKomsu5.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell3 = hexKomsu5;
                    }
                    else if (hexKomsu5 && hexKomsu0)
                    {
                        clearAllSelectCell(this);
                        selectBorder();
                        hexKomsu5.selected = true;
                        hexKomsu5.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell2 = hexKomsu5;
                        hexKomsu0.selected = true;
                        hexKomsu0.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell3 = hexKomsu0;
                    }
                }
                else if (_randomNeighbor == 3)
                {
                    if (hexKomsu3 && hexKomsu4)
                    {
                        clearAllSelectCell(this);
                        selectBorder();
                        hexKomsu3.selected = true;
                        hexKomsu3.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell2 = hexKomsu3;
                        hexKomsu4.selected = true;
                        hexKomsu4.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell3 = hexKomsu4;
                    }
                    else if (hexKomsu4 && hexKomsu5)
                    {
                        clearAllSelectCell(this);
                        selectBorder();
                        hexKomsu4.selected = true;
                        hexKomsu4.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell2 = hexKomsu4;
                        hexKomsu5.selected = true;
                        hexKomsu5.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell3 = hexKomsu5;
                    }
                }
                else if (_randomNeighbor == 4)
                {
                    if (hexKomsu4 && hexKomsu5)
                    {
                        clearAllSelectCell(this);
                        selectBorder();
                        hexKomsu4.selected = true;
                        hexKomsu4.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell2 = hexKomsu4;
                        hexKomsu5.selected = true;
                        hexKomsu5.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell3 = hexKomsu5;
                    }
                }
                else if (_randomNeighbor == 5)
                {
                    if (hexKomsu5 && hexKomsu0)
                    {
                        clearAllSelectCell(this);
                        selectBorder();
                        hexKomsu5.selected = true;
                        hexKomsu5.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell2 = hexKomsu5;
                        hexKomsu0.selected = true;
                        hexKomsu0.hexBorder.SetActive(true);
                        HexGrid.Instance.selectedCell3 = hexKomsu0;
                    }
                }

                /*
                Diziler ile;
                if (hexNeighbors[_randomNeighbor] && hexNeighbors[_randomNeighbor+1])
                {
                    clearAllSelectCell();
                    selectBorder();
                    hexNeighbors[_randomNeighbor].selected = true;
                    hexNeighbors[_randomNeighbor].hexBorder.SetActive(true);
                    hexNeighbors[_randomNeighbor+1].selected = true;
                    hexNeighbors[_randomNeighbor+1].hexBorder.SetActive(true);
                }*/
            }
            else
            {
                clearAllSelectCell(this);
                HexGrid.Instance.selectedCell1 = this;
                selected = true;
                selectCellAndNeighbors();                
            }            
        }

        /// <summary>
        /// Sürükleme iþlemi baþlangýcý
        /// </summary>
        /// <param name="eventData">Pointer event data</param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            //Debug.Log("Drag Start!");
            //bas-sürükle yön belirlemesi için ilk basýlan yerin koordinatý
            _vposition1 = eventData.pressPosition.y;
            //Debug.Log(_vposition1);
        }

        /// <summary>
        /// Sürükleme iþlemi
        /// </summary>
        /// <param name="eventData">Pointer event data</param>
        public void OnDrag(PointerEventData eventData)
        {
            //Debug.Log("OnDrag");
        }

        /// <summary>
        /// Sürükleme iþlemi bitimi
        /// </summary>
        /// <param name="eventData">Pointer event data</param>
        public void OnEndDrag(PointerEventData eventData)
        {
            //bas-sürükle yön belirlemesi için son noktanýn koordinatý
            _vposition2 = eventData.position.y;
            bool _vdirectionDown = true;
            if (_vposition1<_vposition2)
            {
                //Debug.Log("up!");
                _vdirectionDown = false;
            }
            else
            {
                //Debug.Log("down!");
                _vdirectionDown = true;
            }
            
            if (checkSelectedAllCell())
            {

                HexGrid.Instance.swapCell(HexGrid.Instance.selectedCell1,HexGrid.Instance.selectedCell2,HexGrid.Instance.selectedCell3, _vdirectionDown);

                /*
                 * seçili hücrelerin yerlerinin deðiþtirilerek döndürme efekti için;
                 * kullanmak için renk deðiþimi yerine index ve colorindex deðiþimi yapýlmalý
                 * tekrar sürükle-býrak yapmayý engellemeli
                iTween.MoveTo(HexGrid.Instance.selectedCell1.gameObject, iTween.Hash("position", HexGrid.Instance.selectedCell2.gameObject.transform.position));
                iTween.MoveTo(HexGrid.Instance.selectedCell2.gameObject, iTween.Hash("position", HexGrid.Instance.selectedCell3.gameObject.transform.position));
                iTween.MoveTo(HexGrid.Instance.selectedCell3.gameObject, iTween.Hash("position", HexGrid.Instance.selectedCell1.gameObject.transform.position));
                */

            }
            //Debug.Log("Drag End!");
        }

        /// <summary>
        /// Mouse iþlemleri
        /// </summary>
        /// <param name="eventData">Pointer event data</param>
        public void OnPointerDown(PointerEventData eventData)
        {
            //Html5 versiyon yapýlýrsa komþu göstergesi sýfýrlama burada olacak
            //Debug.Log("OnPointerDown");
        }

        /// <summary>
        /// Mouse iþlemleri
        /// </summary>
        /// <param name="eventData">Pointer event data</param>
        public void OnPointerUp(PointerEventData eventData)
        {
            //Html5 versiyon yapýlýrsa komþu göstergesi burada olacak
            //Debug.Log("OnPointerUp");
        }

        /// <summary>
        /// Seçilen hücreyi ve komþularýný seçili gösterir (saat yönünde ve sýralý)
        /// </summary>
        public void selectCellAndNeighbors()
        {
            //Debug.Log("selectCellAndNeighbors()");
            for (int i = 0; i <= 5; i++)
            {
                if (hexKomsu0)
                {
                    //Debug.Log("komþu 0");
                    hexKomsu0.hexBorder.SetActive(true);//i
                    hexKomsu0.selected = true;//i
                    HexGrid.Instance.selectedCell2 = hexKomsu0;
                    if (hexKomsu1)//i+1
                    {
                        hexKomsu1.hexBorder.SetActive(true);//i+1
                        hexKomsu1.selected = true;//i+1
                        HexGrid.Instance.selectedCell3 = hexKomsu1;
                    }
                    else if (hexKomsu2)//i+2
                    {
                        hexKomsu2.hexBorder.SetActive(true);//i+2
                        hexKomsu2.selected = true;//i+2
                        HexGrid.Instance.selectedCell3 = hexKomsu2;
                    }
                    else if (hexKomsu3)//i+3
                    {
                        //sol baþtaki hücre seçilirse
                        if (i == 0)
                        {
                            //seçimi iptal et ve 3. ve 4.komþu hücre ile devam et
                            Debug.Log("sol baþ!");
                            hexKomsu0.hexBorder.SetActive(false);//i
                            hexKomsu0.selected = false;//i
                            
                            hexKomsu3.hexBorder.SetActive(true);//i+3
                            hexKomsu3.selected = true;//i+3
                            HexGrid.Instance.selectedCell2 = hexKomsu3;
                            hexKomsu4.hexBorder.SetActive(true);//i+4
                            hexKomsu4.selected = true;//i+4
                            HexGrid.Instance.selectedCell3 = hexKomsu4;
                        }
                        else
                        {
                            hexKomsu3.hexBorder.SetActive(true);//i+3
                            hexKomsu3.selected = true;//i+3
                            HexGrid.Instance.selectedCell3 = hexKomsu3; /////////////
                        }
                    } else if (hexKomsu4)//i+4
                    {
                        hexKomsu4.hexBorder.SetActive(true);//i+4
                        hexKomsu4.selected = true;//i+4
                        HexGrid.Instance.selectedCell3 = hexKomsu4; ///////////////////
                    } else if (hexKomsu5)//i+5
                    {
                        hexKomsu5.hexBorder.SetActive(true);//i+5
                        hexKomsu5.selected = true;//i+5
                        HexGrid.Instance.selectedCell3 = hexKomsu5; /////////////////
                    }
                    break;
                }
                if (hexKomsu1)
                {
                    //Debug.Log("komþu 1");
                    hexKomsu1.hexBorder.SetActive(true);//i
                    hexKomsu1.selected = true;//i
                    HexGrid.Instance.selectedCell2 = hexKomsu1;
                    if (hexKomsu2)//i+1
                    {
                        hexKomsu2.hexBorder.SetActive(true);//i+1
                        hexKomsu2.selected = true;//i+1
                        HexGrid.Instance.selectedCell3 = hexKomsu2;
                    }
                    else if (hexKomsu3)//i+2
                    {
                        hexKomsu3.hexBorder.SetActive(true);//i+2
                        hexKomsu3.selected = true;//i+2
                        HexGrid.Instance.selectedCell3 = hexKomsu3;
                    }
                    else if (hexKomsu4)//i+3
                    {
                        //sol baþtaki hücre seçilirse
                        if (i == 0)
                        {
                            //seçimi iptal et ve 3. ve 4.komþu hücre ile devam et
                            Debug.Log("sol baþ!");
                            hexKomsu1.hexBorder.SetActive(false);//i
                            hexKomsu1.selected = false;//i

                            hexKomsu4.hexBorder.SetActive(true);//i+3
                            hexKomsu4.selected = true;//i+3
                            HexGrid.Instance.selectedCell2 = hexKomsu4;
                            hexKomsu5.hexBorder.SetActive(true);//i+4
                            hexKomsu5.selected = true;//i+4
                            HexGrid.Instance.selectedCell3 = hexKomsu5;
                        }
                        else
                        {
                            hexKomsu4.hexBorder.SetActive(true);//i+3
                            hexKomsu4.selected = true;//i+3
                            HexGrid.Instance.selectedCell3 = hexKomsu4;
                        }
                    }
                    else if (hexKomsu5)//i+4
                    {
                        hexKomsu5.hexBorder.SetActive(true);//i+4
                        hexKomsu5.selected = true;//i+4
                        HexGrid.Instance.selectedCell3 = hexKomsu5;
                    }
                    break;
                }
                if (hexKomsu2)
                {
                    //Debug.Log("komþu 2");
                    hexKomsu2.hexBorder.SetActive(true);//i
                    hexKomsu2.selected = true;//i
                    HexGrid.Instance.selectedCell2 = hexKomsu2;
                    if (hexKomsu3)//i+1
                    {
                        hexKomsu3.hexBorder.SetActive(true);//i+1
                        hexKomsu3.selected = true;//i+1
                        HexGrid.Instance.selectedCell3 = hexKomsu3;
                    }
                    else if (hexKomsu4)//i+2
                    {
                        hexKomsu4.hexBorder.SetActive(true);//i+2
                        hexKomsu4.selected = true;//i+2
                        HexGrid.Instance.selectedCell3 = hexKomsu4;
                    }                                       
                    break;
                }
                if (hexKomsu3)
                {
                    //Debug.Log("komþu 3");
                    hexKomsu3.hexBorder.SetActive(true);//i
                    hexKomsu3.selected = true;//i
                    HexGrid.Instance.selectedCell2 = hexKomsu3;
                    if (hexKomsu4)//i+1
                    {
                        hexKomsu4.hexBorder.SetActive(true);//i+1
                        hexKomsu4.selected = true;//i+1
                        HexGrid.Instance.selectedCell3 = hexKomsu4;
                    }
                    else if (hexKomsu5)//i+2
                    {
                        hexKomsu5.hexBorder.SetActive(true);//i+2
                        hexKomsu5.selected = true;//i+2
                        HexGrid.Instance.selectedCell3 = hexKomsu5;
                    }                    
                    break;
                }
                if (hexKomsu4)
                {
                    //Debug.Log("komþu 4");
                    hexKomsu4.hexBorder.SetActive(true);//i
                    hexKomsu4.selected = true;//i
                    HexGrid.Instance.selectedCell2 = hexKomsu4;
                    if (hexKomsu5)//i+1
                    {
                        hexKomsu5.hexBorder.SetActive(true);//i+1
                        hexKomsu5.selected = true;//i+1
                        HexGrid.Instance.selectedCell3 = hexKomsu5;
                    }
                    break;
                }
                if (hexKomsu5)
                {
                    //Debug.Log("komþu 5");
                    hexKomsu5.hexBorder.SetActive(true);//i
                    hexKomsu5.selected = true;//i
                    HexGrid.Instance.selectedCell2 = hexKomsu5; /////////////

                    break;
                }
            }
            /*
            Diziler ile;
            for (int i=0;i<=hexNeighbors.Length-1;i++)
            {
                if(hexNeighbors[i])
                {
                    //Debug.Log("Ýlk bulunan komþu: " + hexNeighbors[i].name + " i:" + i);
                    hexNeighbors[i].hexBorder.SetActive(true);
                    hexNeighbors[i].selected = true;
                    if (hexNeighbors[i + 1])
                    {
                        hexNeighbors[i + 1].hexBorder.SetActive(true);
                        hexNeighbors[i + 1].selected = true;
                    }
                    else if (hexNeighbors[i + 2]) { 
                        hexNeighbors[i + 2].hexBorder.SetActive(true);
                        hexNeighbors[i + 2].selected = true;
                    }
                    else if (hexNeighbors[i + 3])
                    {
                        //sol baþtaki hücre seçilirse
                        if (i == 0)
                        {
                            //seçimi iptal et ve 3. ve 4.komþu hücre ile devam et
                            hexNeighbors[i].hexBorder.SetActive(false);
                            hexNeighbors[i].selected = false;

                            hexNeighbors[i + 3].hexBorder.SetActive(true);
                            hexNeighbors[i + 3].selected = true;
                            hexNeighbors[i + 4].hexBorder.SetActive(true);
                            hexNeighbors[i + 4].selected = true;
                        }
                        else
                        {
                            hexNeighbors[i + 3].hexBorder.SetActive(true);
                            hexNeighbors[i + 3].selected = true;
                        }
                    }
                    else if (hexNeighbors[i + 4])
                    {
                        hexNeighbors[i + 4].hexBorder.SetActive(true);
                        hexNeighbors[i + 4].selected = true;
                    }
                    else if (hexNeighbors[i + 5])
                    {
                        hexNeighbors[i + 5].hexBorder.SetActive(true);
                        hexNeighbors[i + 5].selected = true;
                    }

                    break;
                }
            }
            */
            selectBorder();
        }

        /// <summary>
        /// Seçili olan hücrenin kenarlýklarýný açar/kapatýr. GameObject Border tagý "Border" olmalý!
        /// </summary>
        public void selectBorder()
        {
            GameObject border = GameObjectUtility.GetChildrenByTag(gameObject, "Border");
            if (border != null)
            {
                if (border.activeSelf == false)
                {
                    border.gameObject.SetActive(true);
                }
                else
                {
                    border.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Seçilen hücre dýþýndaki tüm hücrelerin seçimini temizler
        /// </summary>
        /// <param name="cell">Hücre</param>
        public void clearAllSelectCell(HexCell cell)
        {
            HexGrid.Instance.selectedCell1 = null;
            HexGrid.Instance.selectedCell2 = null;
            HexGrid.Instance.selectedCell3 = null;
            for (int i = 0; i <= HexGrid.Instance.m_cells.Length - 1; i++)
            {
                HexGrid.Instance.m_cells[i].hexBorder.SetActive(false);
                HexGrid.Instance.m_cells[i].selected = false;
            }
            if (cell != null)
            {
                cell.selected = true;
                HexGrid.Instance.selectedCell1 = cell;
            }
        }

        /// <summary>
        /// Tüm hücrelerin seçim durumunu kontrol eder
        /// </summary>
        /// <returns>_checkValue = true/false</returns>
        public bool checkSelectedAllCell()
        {
            for (int i = 0; i <= HexGrid.Instance.m_cells.Length - 1; i++)
            {
                if (HexGrid.Instance.m_cells[i].selected == true)
                {
                    _checkValue = true;
                    break;
                }
                else _checkValue = false;
            }
            return _checkValue;
        }

    }
}