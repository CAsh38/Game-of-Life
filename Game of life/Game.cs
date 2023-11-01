using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace Game_of_life
{
    public class Game
    {
        public bool[,] Matrice { get; set; }
        public List<Cell> Live_Cells { get; set; }
        public System.Drawing.Brush Brush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
        public List<Cell> Get_all_neighbouring_cells()
        {
            //Here we are getting all the neighbouring cells of all the cells alive with no duplicates
            var neighbours_list = new List<Cell>();
            bool check_for_duplicates(int cell_x,int cell_y)
            {
                foreach(Cell dup_cell in neighbours_list)
                {
                    if (dup_cell.X == cell_x && dup_cell.Y == cell_y) return false;
                }
                return true;
            }
            foreach (var cell in Live_Cells)
            {
                var neighbours_matrice = Get_cell_neighbours(cell);
                for(int i = 0; i < neighbours_matrice.Length; i+=2)
                {
                    try
                    {
                        if (Matrice[neighbours_matrice[i / 2, 0], neighbours_matrice[i / 2, 1]])
                        {
                            continue;
                        }
                        else if (check_for_duplicates(neighbours_matrice[i / 2, 0], neighbours_matrice[i / 2, 1]))
                        {
                            var neigh_cell = new Cell
                            {
                                X = neighbours_matrice[i / 2, 0],
                                Y = neighbours_matrice[i / 2, 1]
                            };
                            neighbours_list.Add(neigh_cell);
                        }
                    }
                    catch { continue; }
                }
            }
            return neighbours_list;
        }
        public int[,] Get_cell_neighbours(Cell cell)
        {
            //Here we are getting the neighbours of one cell
            var matrice = new int[8,2];
            matrice[0,0] = cell.X-1;
            matrice[0,1] = cell.Y-1;
            matrice[1,0] = cell.X;
            matrice[1,1] = cell.Y-1;
            matrice[2,0] = cell.X+1;
            matrice[2,1] = cell.Y-1;
            matrice[3,0] = cell.X-1;
            matrice[3,1] = cell.Y;
            matrice[4,0] = cell.X+1;
            matrice[4,1] = cell.Y;
            matrice[5,0] = cell.X-1;
            matrice[5,1] = cell.Y+1;
            matrice[6,0] = cell.X;
            matrice[6,1] = cell.Y+1;
            matrice[7,0] = cell.X+1;
            matrice[7,1] = cell.Y+1;
            return matrice;
        } 
        public void Update_Live_Cells()
        {
            // Here we update the cells getting first the neighbouring cells then running through all of them 
            var neighbours = Get_all_neighbouring_cells();
            var new_list = new List<Cell>();
            foreach (Cell cell in Live_Cells)
            {
                var cell_neighbours = Get_cell_neighbours(cell);
                int k = 0;
                for (int i = 0; i < cell_neighbours.Length; i += 2)
                {
                    try
                    {
                        if (Matrice[cell_neighbours[i / 2, 0], cell_neighbours[i / 2, 1]]) k++;
                    }
                    catch { continue; }
                }
                if (k <= 1) { continue; }
                else if (k <= 3)
                {
                    new_list.Add(cell);
                }
            }
            foreach(Cell cell in neighbours)
            {
                var cell_neighbours = Get_cell_neighbours(cell);
                int k = 0;
                for (int i = 0; i < cell_neighbours.Length; i += 2)
                {
                    try
                    {
                        if (Matrice[cell_neighbours[i / 2, 0], cell_neighbours[i / 2, 1]]) k++;
                    }
                    catch { continue; }
                }
                if (k == 3)
                {
                    new_list.Add(cell);
                }
            }
            Live_Cells = new_list;
            Matrice = new bool[75, 52];
            foreach (Cell cell in Live_Cells)
            {
                Matrice[cell.X, cell.Y] = true;
            }
        }
        public void Create_Glider(int cell_X,int cell_Y)
        {
            // Custom method to check if it runs correctly 
            Matrice = new bool[75, 52];
            Live_Cells = new List<Cell>();
            var Cell_1 = new Cell
            {
                X = cell_X + 1,
                Y = cell_Y
            };
            Live_Cells.Add(Cell_1);
            Matrice[Cell_1.X, Cell_1.Y] = true;
            var Cell_2 = new Cell
            {
                X = cell_X + 2,
                Y = cell_Y + 1
            };
            Live_Cells.Add(Cell_2);
            Matrice[Cell_2.X, Cell_2.Y] = true;
            var Cell_3 = new Cell
            {
                X = cell_X,
                Y = cell_Y + 2
            };
            Live_Cells.Add(Cell_3);
            Matrice[Cell_3.X, Cell_3.Y] = true;
            var Cell_4 = new Cell
            {
                X = cell_X + 1,
                Y = cell_Y + 2
            };
            Live_Cells.Add(Cell_4);
            Matrice[Cell_4.X, Cell_4.Y] = true;
            var Cell_5 = new Cell
            {
                X = cell_X + 2,
                Y = cell_Y + 2
            };
            Live_Cells.Add(Cell_5);
            Matrice[Cell_5.X, Cell_5.Y] = true;
        }
        public void Populate_with_cells(int number)
        {
            //Randomly get n number of cells 

            Random rnd = new Random();
            List<int> get_list()
            {
                List<int> list = new List<int>
                {
                    rnd.Next(75),
                    rnd.Next(52)
                };
                return list;
            }
            bool check_for_duplicates(List<int> last_list,List<List<int>> matrice)
            {
                foreach(List<int> list in matrice)
                {
                    if (list[0] == last_list[0] && list[1] == last_list[1]) 
                        return true;
                }
                return false;
            }
            List<List<int>> Coord_Matrice = new List<List<int>>();
            for(int i = 0;i < number; i++)
            {
                var Last_list = get_list();
                while (check_for_duplicates(Last_list,Coord_Matrice))
                {
                    Last_list = get_list();
                }
                Coord_Matrice.Add(Last_list);
            }
            List<Cell> TheList = new List<Cell>();
            Matrice = new bool[75, 52];
            foreach (List<int> list in Coord_Matrice)
            {
                var cell = new Cell
                {
                    X = list[0],
                    Y = list[1]
                };
                Matrice[cell.X,cell.Y] = true;
                TheList.Add(cell);
            }
            Live_Cells = TheList;
        }
    }
}
/*Work witk a matrice and a list
 * Start by creating a matrice of bool values 75 by 52
 * Set all elements of the matrice to false meaning 0
 * By creating a list of cells with the function Populate_cells run through every cell in the list and change false to true in the matrice 
 * By this we save the coordinates of every cell 
 * When we run through every rule we use the matrice to quickly access anyother cell updating the list of cells 
 * Then we update the matrice setting every element to false then run again the list to encode the coordinates to repeat the process*/