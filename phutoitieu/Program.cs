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
                        {
                            // Kiểm tra xem thuộc tính của vế phải đã có trong bao đóng hay chưa
                            if (!SoSanhChuoi(phai[i][j].ToString(), baoDong))
                            {
                                baoDong += phai[i][j].ToString();
                            }
                        }
                    }
                }
            }

            return baoDong;
        }

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

            return ChuoiCon == con.Length;
        }

        public S_PhuToiThieu TimPhuToiThieu(List<string> trai, List<string> phai)
        {
            S_PhuToiThieu ptt = new S_PhuToiThieu();

            int n = phai.Count;
            // Tách phụ thuộc hàm vế phải có hơn 1 thuộc tính
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

            // Loại bỏ thuộc tính dư thừa bên vế trái có hơn 1 thuộc tính
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

            // Loại bỏ thuộc tính dư thừa
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

        public List<string> TimTapCon(string tap)
        {
            List<string> tapCon = new List<string>();
            int n = tap.Length;

            // Generate all subsets of the given set
            for (int i = 0; i < (1 << n); i++)
            {
                string subset = "";
                for (int j = 0; j < n; j++)
                {
                    if ((i & (1 << j)) > 0)
                        subset += tap[j];
                }
                if (!string.IsNullOrEmpty(subset))
                    tapCon.Add(subset);
            }

            return tapCon;
        }

        public List<string> TimKhoa(string tapThuocTinh, List<string> trai, List<string> phai)
        {
            List<string> listKhoa = new List<string>();
            List<string> tapCon = TimTapCon(tapThuocTinh);

            foreach (string subset in tapCon)
            {
                // Calculate the closure of the subset
                string baoDong = TimBaoDong(subset, trai, phai);

                // If the closure of the subset contains all attributes, it is a superkey
                if (SoSanhChuoi(tapThuocTinh, baoDong))
                {
                    bool isMinimal = true;
                    // Check if there is a smaller subset that is also a superkey
                    foreach (string smallerSubset in TimTapCon(subset))
                    {
                        if (smallerSubset.Length < subset.Length &&
                            SoSanhChuoi(tapThuocTinh, TimBaoDong(smallerSubset, trai, phai)))
                        {
                            isMinimal = false;
                            break;
                        }
                    }

                    // If no smaller subset is a superkey, it is a candidate key
                    if (isMinimal)
                    {
                        listKhoa.Add(subset);
                    }
                }
            }

            return listKhoa;
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            C_ThuatToan thuatToan = new C_ThuatToan();
            List<string> trai = new List<string>();
            List<string> phai = new List<string>();

            Console.WriteLine("Nhap tap thuoc tinh (VD: ABCD):");
            string tapThuocTinh = Console.ReadLine();

            Console.WriteLine("So luong phu thuoc ham:");
            int soPhuThuocHam = int.Parse(Console.ReadLine());

            for (int i = 0; i < soPhuThuocHam; i++)
            {
                Console.WriteLine($"Nhap phu thuoc ham {i + 1} (VD: A->BC):");
                string phuThuoc = Console.ReadLine();


                string veTrai = "";
                string vePhai = "";

                bool isVeTrai = true;  // kiểm tra có  ở vế trái không

                // Duyệt từng ký tự trong phụ thuộc hàm
                for (int j = 0; j < phuThuoc.Length; j++)
                {
                    if (j < phuThuoc.Length - 1 && phuThuoc[j] == '-' && phuThuoc[j + 1] == '>')
                    {
                        isVeTrai = false; 
                        j++; 
                    }
                    else
                    {
                        if (isVeTrai) //nếu cờ là đúng thì add kí tự đang xem vào mảng vế trái
                            veTrai += phuThuoc[j];
                        else    // còn không thì add vô vế phải
                            vePhai += phuThuoc[j];
                    }
                }

                if (veTrai == "" || vePhai == "")
                {
                    Console.WriteLine("Phu thuoc ham khong hop le. Vui long nhap lai.");
                    i--; 
                }
                else
                {
                    trai.Add(veTrai);
                    phai.Add(vePhai);
                }
            }

            S_PhuToiThieu phuToiThieu = thuatToan.TimPhuToiThieu(trai, phai);

            Console.WriteLine("\nPhu toi tieu la:");
            for (int i = 0; i < phuToiThieu.trai.Count; i++)
            {
                Console.WriteLine($"{phuToiThieu.trai[i]} -> {phuToiThieu.phai[i]}");
            }

            List<string> khoaUngVien = thuatToan.TimKhoa(tapThuocTinh, phuToiThieu.trai, phuToiThieu.phai);
            Console.WriteLine("\nCac khoa ung vien la:");
            foreach (string khoa in khoaUngVien)
            {
                Console.WriteLine(khoa);
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
