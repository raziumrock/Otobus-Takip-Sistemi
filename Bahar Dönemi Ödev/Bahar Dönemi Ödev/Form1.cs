using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bahar_Dönemi_Ödev // Dosyanın adı
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void cmbOtobus_SelectedIndexChanged(object sender, EventArgs e) 
        {
            switch(cmbOtobus.Text) // otobüs seçiniz adlı combobox'ına bastığımızda görünen seçenekler
            { // Parantez içindeki sayılar bulunan sıra sayısıdır.
                case "Kamil Koc":KoltukDoldur(8, false); // Örneğin Kamil koç firmasının otobüslerinde 8 sıra koltuk bulunmaktadır.
                        break;
                case "Metro Turizm":KoltukDoldur(12, true);// Metro Turizm'de bu sıra koltuk sayısı 12'dir.
                    break;
                case"Öz Sivas":KoltukDoldur(10, false);// Öz Sivas firmasında ise 10 sıra koltuk vardır.
                    break;
                case "Ulusoy Turizm":KoltukDoldur(15, true);// Ulusoy firmasında ise 15 sıra koltuk vardır.
                    break;
                case "Pamukkale":KoltukDoldur(8, false);// Pamukkale firmasında ise 8 sıra koltuk vardır.
                    break;

            }
            void KoltukDoldur(int sira,bool arkabeslimi) // sira değişkenini integer olarak tanıttık, arka sıranın beşli olup olmadığı true false olduğu için bool olarak atadık
            {
                yavaslat: // Bu komut koltuk seçme sırasında firma seçip değiştirdiğimizde koltuk numara butonlarının silinmemesi için
                foreach(Control ctrl in this.Controls)
                {
                    if(ctrl is Button) // Kontrol edilen değişken buton ise
                    {
                        Button btn = ctrl as Button;
                        if(btn.Text=="Rezerve Et") // Kontrol edilen buton "Rezerve Et" butonu ise devam eder
                        {
                            continue;
                        }
                        else
                        {
                            this.Controls.Remove(ctrl); //  Rezerve et butonuna basılmadıysa otobüs firması seçim bölümüne geri gönderir 
                            goto yavaslat;
                        }
                    }
                }
                int koltukNo = 1;
                for(int i=0;i<sira;i++) // Bu for döngüsü koltuk sıralarının dikey olarak hizalandırılması için
                {
                    for(int j=0;j<5;j++) // Bu for döngüsü ise koltukların yatay olarak hizalandırılması için
                    {
                        if(arkabeslimi==true) // Arka sıradaki koltuklar beşli ise 
                        {
                            if(i !=sira-1 &&j==2) // Orta sıra boş arka taraftaki sıra koltuklar beşli
                            {
                                continue;
                            }
                        }
                        else
                        {
                            if (j == 2) // Orta sıra tamamen boş, koltuk bulunmamakta
                                continue;
                        }
                        
                        Button koltuk = new Button();
                        koltuk.Height = koltuk.Width = 40; // Koltukların boyutu 40 birim olucak
                        koltuk.Top = 30 + (i * 45); // Otobüs seçtikten sonraki koltuk butonlarının yukarıdan 30 birim aşağıdan başlamasına yarıyor
                        koltuk.Left = 5 + (j * 45); // Otobüs seçtikten sonraki koltuk butonlarının sol taraftan 5 birim sağ taraftan başlamasına yarıyor
                        koltuk.Text = koltukNo.ToString(); 
                        koltukNo++; // Koltuk numarasını birer birer arttırmak için
                        koltuk.ContextMenuStrip = contextMenuStrip1;
                        koltuk.MouseDown += Koltuk_MouseDown;// Mouse ile sağ click yaptığımızda "Rezerve Et" bölümü çıkar ve koltuk eklemek için KayitFormu kısmına gönderir
                        this.Controls.Add(koltuk); // Mouse ile sol click yapıldığında "koltuk" seçimi yapılır


                    }
                }
            }
        }
        Button tiklanan; // Rezerve et butonu için 
        private void Koltuk_MouseDown(object sender, MouseEventArgs e)
        {
            tiklanan = sender as Button; // Rezerve et butonu için
        }

        private void rezerveEkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(cmbOtobus.SelectedIndex==-1 || cmbNereden.SelectedIndex==-1|| cmbNereye.SelectedIndex==-1)
            {
                MessageBox.Show("Lütfen önce gerekli alanı doldurunuz"); // Otobüs firması seçmeden, nereden binileceğini ve nereye gidileceğini seçmeden ekrana mesaj kutusu gönderir
                    return;// Mesaj kutusu geldikten sonra işleme devam eder.

            }
            KayitFormu kf = new KayitFormu(); 
            DialogResult sonuc=kf.ShowDialog();
            if(sonuc==DialogResult.OK) // Eğer Kayıt formu başarıyla tamamlanırsa
            {
                ListViewItem lvi = new ListViewItem(); // ListView'da bir satır eklenir
                lvi.Text = string.Format("{0} {1}", kf.txtİsim.Text, kf.txtSoyisim.Text); // İsim ve Soyisim ilgili bölüme eklenir
                lvi.SubItems.Add(kf.mskdTelefon.Text); // Telefon numarası ilgili bölüme eklenir
                if(kf.rdbBay.Checked) // Eğer BAY butonu seçildiyse
                {
                    lvi.SubItems.Add("BAY"); // Listview'da cinsiyet bölümüne BAY eklenir
                    tiklanan.BackColor = Color.Blue; // Seçilen koltuk numarasının arkaplan rengi mavi olur
                }
                if(kf.rdbBayan.Checked) // Eğer BAYAN butonu seçildiyse
                {
                    lvi.SubItems.Add("BAYAN"); // Listview'da cinsiyet bölümüne BAYAN eklenir
                    tiklanan.BackColor = Color.Red; // Seçilen koltuk numarasının arkaplan rengi kırmızı olur
                }
                lvi.SubItems.Add(cmbNereden.Text); // Nereden kısmı Listview'da ilgili bölüme eklenir
                lvi.SubItems.Add(cmbNereye.Text); // Nereye kısmı Listview'da ilgili bölüme eklenir
                lvi.SubItems.Add(tiklanan.Text); // Seçilen koltuk numarası Listview'da ilgili bölüme eklenir
                lvi.SubItems.Add(dtpTarih.Text); // Seçilen tarih Listview'da ilgili bölüme eklenir
                lvi.SubItems.Add(nudFiyat.Value.ToString()); // Fiyat kısmı Listview'da ilgili bölüme eklenir
                listView1.Items.Add(lvi); // lvi değişkenine girdiğimiz değerler listview'da ilgili bölüme eklenecektir


            }
        }
    }
}
