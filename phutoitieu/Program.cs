using System;
using System.Collections.Generic;

namespace TimKhoa
{
    struct S_PhuToiThieu
    {
        public List<string> trai;
        public List<string> phai;
    }

    class C_ThuatToan
    {
        // Add your previous methods here (TimBaoDong, SoSanhChuoi, TimKhoa, TimTapCon, TimPhuToiThieu, etc.)
        public string TimBaoDong(string baoDong, List<string> trai, List<string> phai)
        {
            int doDaiBaoDong = baoDong.Length - 1;

            while (doDaiBaoDong != baoDong.Length)
            {
                doDaiBaoDong = baoDong.Length;

                for (int i = 0; i < trai.Count; i++)
                {
                    if (SoSanhChuoi(trai[i], baoDong))
                    {
                        for (int j = 0; j < phai[i].Length; j++)
                            if (!SoSanhChuoi(phai[i][j].ToString(), baoDong))
                                baoDong += phai[i][j].ToString();
                    }
                }

            }

            return baoDong;
        }

        /// <summary>
        /// So sánh chuỗi A có nằm trong chuỗi B không
        /// </summary>
        /// <param name="con">A</param>
        /// <param name="cha">B</param>
        /// <returns>true nếu nằm trong, ngược lại trả về false</returns>
        private bool SoSanhChuoi(string con, string cha)
        {
            int ChuoiCon = 0;

            if (cha.Length < con.Length)
                return false;

            for (int i = 0; i < con.Length; i++)
                for (int j = 0; j < cha.Length; j++)
                {
                    if (con[i] == cha[j])
                    {
                        ChuoiCon++;
                        break;
                    }
                }

            if (ChuoiCon == con.Length)
                return true;

            return false;
        }

        /// <summary>
        /// tìm tất cả các khóa của lược đồ
        /// </summary>
        /// <param name="tapThuocTinh">tập thuộc tính của lược đồ</param>
        /// <param name="trai">danh sách phụ thuộc hàm bên trái</param>
        /// <param name="phai">danh sách phụ thuộc hàm bên phải</param>
        /// <returns>trả về danh sách các khóa</returns>

        public S_PhuToiThieu TimPhuToiThieu(List<string> trai, List<string> phai)
        {
            S_PhuToiThieu ptt = new S_PhuToiThieu();

            int n = phai.Count;
            //tách phụ thuộc hàm vế phải có hơn 1 thuộc tính
            for (int i = 0; i < n; i++)
            {
                if (phai[i].Length > 1)
                {
                    string tempPhai = phai[i];
                    string temTrai = trai[i];

                    trai.Remove(trai[i]);
                    phai.Remove(phai[i]);

                    for (int j = 0; j < tempPhai.Length; j++)
                    {
                        trai.Add(temTrai);
                        phai.Add(tempPhai[j].ToString());
                    }

                    i--;
                }
            }

            //loại bỏ thuộc tính dư thừa bên vế trái có hơn 1 thuộc tính
            for (int i = 0; i < trai.Count; i++)
            {
                if (trai[i].Length > 1)
                {

                    for (int j = 0; j < trai[i].Length; j++)
                    {
                        if (trai[i].Length > 1)
                        {
                            string temp = trai[i];
                            temp = CatKiTu(temp, j);

                            if (SoSanhChuoi(phai[i], TimBaoDong(temp, trai, phai)))
                            {
                                trai[i] = temp;
                                j--;
                            }

                        }
                    }
                }
            }

            //loại bỏ thuộc tính dư thừa
            List<string> TempTrai = new List<string>();
            List<string> TempPhai = new List<string>();

            for (int i = 0; i < trai.Count; i++)
            {
                TempTrai.Add(trai[i]);
                TempPhai.Add(phai[i]);
            }

            for (int i = 0; i < trai.Count; i++)
            {

                TempTrai.RemoveAt(i);
                TempPhai.RemoveAt(i);

                if (SoSanhChuoi(phai[i], TimBaoDong(trai[i], TempTrai, TempPhai)))
                {
                    trai.Clear();
                    phai.Clear();

                    for (int t = 0; t < TempTrai.Count; t++)
                    {
                        trai.Add(TempTrai[t]);
                        phai.Add(TempPhai[t]);
                    }

                    i--;
                }
                else
                {
                    TempTrai.Clear();
                    TempPhai.Clear();

                    for (int t = 0; t < trai.Count; t++)
                    {
                        TempTrai.Add(trai[t]);
                        TempPhai.Add(phai[t]);
                    }
                }
            }

            ptt.trai = trai;
            ptt.phai = phai;

            return ptt;
        }

        // Cut character helper method
        public string CatKiTu(string str, int vitri)
        {
            string ok = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (vitri != i)
                    ok += str[i].ToString();
            }
            return ok;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            C_ThuatToan thuatToan = new C_ThuatToan();
            List<string> trai = new List<string>();
            List<string> phai = new List<string>();

            // Input attributes and functional dependencies
            Console.WriteLine("Nhap tap thuoc tinh (VD: ABCD):");
            string tapThuocTinh = Console.ReadLine();

            Console.WriteLine("So luong phu thuoc ham:");
            int soPhuThuocHam = int.Parse(Console.ReadLine());

            for (int i = 0; i < soPhuThuocHam; i++)
            {
                Console.WriteLine($"Nhap ve trai cua phu thuoc ham {i + 1} (VD: A):");
                string traiPhuThuoc = Console.ReadLine();
                Console.WriteLine($"Nhap ve phai cua phu thuoc ham {i + 1} (VD BC):");
                string phaiPhuThuoc = Console.ReadLine();

                trai.Add(traiPhuThuoc);
                phai.Add(phaiPhuThuoc);
            }

            // Calculating minimal cover
            S_PhuToiThieu phuToiThieu = thuatToan.TimPhuToiThieu(trai, phai);

            // Displaying the minimal cover
            Console.WriteLine("\nPhu toi tieu la:");
            for (int i = 0; i < phuToiThieu.trai.Count; i++)
            {
                Console.WriteLine($"{phuToiThieu.trai[i]} -> {phuToiThieu.phai[i]}");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
