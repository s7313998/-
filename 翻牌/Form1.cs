namespace 翻牌
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    public partial class Form1 : Form
    {
        //放button
        Button[] buttons;
        //各2張一樣的牌
        string[] cards =
            {
                "A","A",
                "B","B",
                "C","C",
                "D","D",
            };
        //第一次按的button
        Button firstButton;
        //第一次按button位置
        int firstIndex = -1;
        //是否等待
        bool waiting;
        //配對次數
        int match = 0;
        //嘗試次數
        int count = 0;
        //紀錄時機間
        int seconds = 0;
        //隨機
        Random random = new Random();
        public Form1()
        {
            InitializeComponent();

            //按鈕
            buttons = new Button[]
            {
                button1, button2, button3, button4, button5, button6,button7,button8
            };
            //設定button
            foreach (Button button in buttons)
            {
                //點button執行Card_Click
                button.Click += Card_Click;
                //牌還沒翻開
                button.Tag = false;
            }

            //洗牌
            Shuffle();
            gametimer.Interval = 1000;
            gametimer.Tick += gametimer_tick;
            label2.Text = "時間:00:00";


        }

        //洗牌
        private void Shuffle()
        {
            for (int i = 0; i < cards.Length; i++)
            {
                int randomIndex = random.Next(cards.Length);
                string temp = cards[i];
                cards[i] = cards[randomIndex];
                cards[randomIndex] = temp;
            }
        }

        //點擊牌
        private void Card_Click(object sender, EventArgs e)
        {
            //如果等待 就不可點
            if (waiting)
                return;
            //找出被點擊的button
            Button button = (Button)sender;
            //如果被點過就不能翻
            if ((bool)button.Tag == true)
            {
                return;
            }
            //找button在陣列中的對應位置
            int index = Array.IndexOf(buttons, button);
            //翻牌圖片
            button.Image = getGetCard(cards[index]);
            //第一張牌
            if (firstButton == null)
            {
                //開始計時
                if (seconds == 0)
                {
                    gametimer.Start();
                }
                //第一張的button
                firstButton = button;
                //第一張的位置
                firstIndex = index;
                //button已被點擊
                button.Tag = true;


            }
            //第二張牌
            else
            {
                //嘗試次數+1
                count++;
                //判斷是否配對成功
                if (cards[firstIndex] == cards[index])
                {
                    //配對成功
                    match++;
                    //清除第一張牌
                    firstButton = null;
                    //清除第一張牌
                    firstIndex = -1;
                    button.Tag = true;
                    if (match == 4)
                    {
                        gametimer.Stop();
                        gametimer.Dispose();
                        label1.Text = "恭喜配對成功!\r\n" + $"總共嘗試{count}次" + label2.Text;
                    }

                }
                //配對失敗
                else
                {
                    waiting = true;
                    Timer timer = new Timer();
                    //1秒
                    timer.Interval = 1000;
                    timer.Tick += (s, args) =>
                    {
                        //第一張牌翻回
                        firstButton.Image = null;
                        //第二章牌翻回
                        button.Image = null;
                        //代表兩張牌可以重新點
                        firstButton.Tag = false;
                        button.Tag = false;
                        //清除第一張牌
                        firstButton = null;
                        firstIndex = -1;
                        //可以繼續點
                        waiting = false;
                        timer.Stop();
                        timer.Dispose();
                    };
                    timer.Start();

                }

            }

        }
        private Image getGetCard(string card)
        {
            switch (card)
            {
                case "A":
                    return new Bitmap(Properties.Resources.Laughcry);
                case "B":
                    return new Bitmap(Properties.Resources.hehe);
                case "C":
                    return new Bitmap(Properties.Resources.sweat);
                case "D":
                    return new Bitmap(Properties.Resources.Smiling);


                default:
                    return null;
            }

        }

        private void gametimer_tick(object sender, EventArgs e)
        {
            //計時
            seconds++;
            //分鐘
            int minutes = seconds / 60;
            //秒數
            int hours = seconds % 60;
            label2.Text = "時間:" + minutes.ToString("00") + ":" + hours.ToString("00");

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
