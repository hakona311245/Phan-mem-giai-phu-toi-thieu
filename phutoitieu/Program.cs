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

        public List<string> TimKhoa(string tapThuocTinh, List<string> trai, List<string> phai)
        {
            List<string> listKhoa = new List<string>();

            string L = "";
            string R = "";
            string TN = "";
            string TG = "";

            //lấy tập L (chỉ xuất hiện vế trái, ko xh vế phải)
            for (int i = 0; i < tapThuocTinh.Length; i++)
            {
                for (int t = 0; t < trai.Count; t++)
                    if (SoSanhChuoi(tapThuocTinh[i].ToString(), trai[t]) && !SoSanhChuoi(tapThuocTinh[i].ToString(), phai[t]))
                    {
                        L += tapThuocTinh[i].ToString();
                        break;
                    }
            }
            //lấy tập R (chỉ xuất hiện vế phải, ko xh vế trái)
            for (int i = 0; i < tapThuocTinh.Length; i++)
            {
                for (int t = 0; t < trai.Count; t++)
                    if (SoSanhChuoi(tapThuocTinh[i].ToString(), phai[t]) && !SoSanhChuoi(tapThuocTinh[i].ToString(), trai[t]))
                    {
                        R += tapThuocTinh[i].ToString();
                        break;
                    }
            }

            /*lấy TN thuộc tính chỉ xuất hiện ở vế trái, không xuất hiện ở vế phải và
             * các thuộc tính không xuất hiện ở cả vế trái và vế phải của F*/
            for (int i = 0; i < tapThuocTinh.Length; i++)
            {
                if (!SoSanhChuoi(tapThuocTinh[i].ToString(), R))
                {
                    TN += tapThuocTinh[i].ToString();
                }
            }
            //lấy TG (giao giữa 2 tập L và R)
            for (int i = 0; i < L.Length; i++)
            {
                if (SoSanhChuoi(L[i].ToString(), R))
                {
                    TG += L[i].ToString();
                }
            }

            //nếu tập TG rỗng thì khóa chính là TN
            if (TG == "")
            {
                listKhoa.Add(TN);
                return listKhoa;
            }
            else
            {
                List<string> TapConTG = new List<string>();
                //sinh tập con của TG
                TapConTG = TimTapCon(TG);

                List<string> SieuKhoa = new List<string>();

                //kiểm tra từng tập con của TG hợp với TN có là siêu khóa không
                for (int n = 0; n < TapConTG.Count; n++)
                {
                    //lấy giao tập nguồn(TN) và từng con của TG 
                    string temp = TN + TapConTG[n];
                    //nếu giao tập nguồn(TN) và từng con của TG tất cả lấy bao đóng mà bằng tập thuộc tính thì là siêu khóa
                    if (SoSanhChuoi(tapThuocTinh, TimBaoDong(temp, trai, phai)))
                    {
                        SieuKhoa.Add(temp);
                    }
                }

                //tìm siêu khóa tối thiểu
                for (int i = 0; i < SieuKhoa.Count; i++)
                {
                    for (int j = i + 1; j < SieuKhoa.Count; j++)
                    {
                        if (SoSanhChuoi(SieuKhoa[i], SieuKhoa[j]))
                        {
                            SieuKhoa.Remove(SieuKhoa[j]);
                            j--;
                        }
                    }
                }

                listKhoa = SieuKhoa;
            }

            return listKhoa;
        }

        /// <summary>
        /// tìm con con bằng phương pháp sinh
        /// </summary>
        /// <param name="str">chuỗi cần sinh tập con</param>
        /// <returns>trả về danh sách tập con</returns>
        List<string> TimTapCon(string str)
        {

            List<string> TapCon = new List<string>();

            int[] a = new int[str.Length];

            for (int i = 0; i < a.Length; i++)
            {
                a[i] = 0;
            }

            int t = str.Length - 1;

            TapCon.Add("");
            while (t >= 0)
            {

                t = str.Length - 1;
                while (t >= 0 && a[t] == 1)
                    t--;

                if (t >= 0)
                {
                    a[t] = 1;
                    for (int i = t + 1; i < str.Length; i++)
                        a[i] = 0;

                    string temp = "";
                    for (int i = 0; i < str.Length; i++)
                    {
                        if (a[i] == 1)
                        {
                            temp += str[i];
                        }
                    }

                    TapCon.Add(temp);
                }
            }

            return TapCon;
        }


        public bool KiemTra1NF(List<string> phai)
        {
            // Kiểm tra 1NF: tất cả các thuộc tính đều phải có giá trị đơn
            return true; // Assuming input is always in 1NF as mentioned
        }

        public bool KiemTra2NF(List<string> trai, List<string> phai, List<string> khoaUngVien)
        {
            // Kiểm tra 2NF: tất cả các thuộc tính không khoá phải phụ thuộc đầy đủ vào khoá
            foreach (var k in khoaUngVien)
            {
                foreach (var f in phai)
                {
                    if (f.Length > 1) // If there are multiple attributes in the right-hand side
                    {
                        // Check for partial dependency
                        string closure = TimBaoDong(k, trai, phai);
                        if (!SoSanhChuoi(closure, TimBaoDong(CatKiTu(k, 0), trai, phai)))
                        {
                            return false; // There is a partial dependency
                        }
                    }
                }
            }
            return true; // No partial dependency found
        }

        public bool KiemTra3NF(List<string> trai, List<string> phai, List<string> khoaUngVien)
        {
            // Kiểm tra 3NF: cho mỗi phụ thuộc hàm X→ A, X là siêu khoá hoặc A là thuộc tính khoá
            foreach (var f in trai)
            {
                string closure = TimBaoDong(f, trai, phai);
                if (!SoSanhChuoi(closure, TimBaoDong(CatKiTu(f, 0), trai, phai)))
                {
                    // A là thuộc tính không phải khoá
                    if (!khoaUngVien.Contains(f))
                        return false; // Not in 3NF
                }
            }
            return true; // In 3NF
        }

        public bool KiemTraBCNF(List<string> trai, List<string> phai)
        {
            // Kiểm tra BCNF: cho mỗi phụ thuộc hàm X → A, X là siêu khoá
            foreach (var f in trai)
            {
                string closure = TimBaoDong(f, trai, phai);
                if (!SoSanhChuoi(closure, TimBaoDong(CatKiTu(f, 0), trai, phai)))
                {
                    return false; // Not in BCNF
                }
            }
            return true; // In BCNF
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

                Console.WriteLine("Kiem tra cac dang chuan:");

                bool is1NF = thuatToan.KiemTra1NF(phai);
                Console.WriteLine($"1NF: {(is1NF ? "Co" : "Khong")}"); // Assuming input is always in 1NF

                bool is2NF = thuatToan.KiemTra2NF(trai, phai, khoaUngVien);
                Console.WriteLine($"2NF: {(is2NF ? "Co" : "Khong")}");

                bool is3NF = thuatToan.KiemTra3NF(trai, phai, khoaUngVien);
                Console.WriteLine($"3NF: {(is3NF ? "Co" : "Khong")}");

                bool isBCNF = thuatToan.KiemTraBCNF(trai, phai);
                Console.WriteLine($"BCNF: {(isBCNF ? "Co" : "Khong")}");

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
