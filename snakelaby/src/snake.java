import javax.swing.*;
import java.awt.*;
import java.awt.event.*;
import java.awt.geom.*;
import java.util.Random;
import javax.imageio.*;
import java.io.*;
import java.awt.image.*;

    public class snake extends JPanel implements ActionListener, KeyListener {

        Timer t = new Timer(125,this);
        int velx=0,vely=0,eat=2;
        int velx2=0,vely2=0,eat2=2;
        private int[] snakex = new int[500];
        private int[] snakey = new int[500];
        private int[] snakex2 = new int[500];
        private int[] snakey2 = new int[500];
        static int t1,t2,t3;
        static int p1,p2,p3,p4,p5,p6;
        boolean loose=false,loose2=false,play=false,frog=false;
        public int score=0;
        public int score2=0;
        private int foodx,foody,boost=1,boost2=1,boostpower=0;
        private boolean leftt=false,rightt=false,upp=false,downn=false;
        private boolean leftt2=false,rightt2=false,upp2=false,downn2=false;
        int wallx = 900, wally = 900, move=1;
        int max = 800, min = 200;
        public boolean multi=false,scoreinfo=false,pause=false,loadg=false;
        private BufferedImage imgfood, imgfood2;
        private BufferedImage imghead, imghead2;
        private BufferedImage imgbody, imgfrog;
        private BufferedImage imgwall;





        public snake(){

            addKeyListener(this);
            setFocusable(true);
            setFocusTraversalKeysEnabled(false);
            t.start();

        }

        public void ima(){
            imgfood = getImage("apple.png");
            imgfood2 = getImage("apple2.png");
            imghead = getImage("head.png");
            imghead2 = getImage("head2.png");
            imgbody = getImage("body.png");
            imgwall = getImage("wall.png");
            imgfrog = getImage("frog.png");
        }
        private BufferedImage getImage(String filename) {
            try {
                InputStream in = getClass().getResourceAsStream(filename);

                return ImageIO.read(in);
            } catch (IOException e) {
                System.out.println("The image was not loaded.");
            }

            return null;
        }


        public void paintComponent(Graphics g) {
                super.paintComponent(g);
                Graphics2D g2 = (Graphics2D) g;
                Graphics2D g3 = (Graphics2D) g;
                Graphics2D g4 = (Graphics2D) g;
                Graphics2D g5 = (Graphics2D) g;
                Graphics2D g6 = (Graphics2D) g;
                ima();

                g.fillRect(0,800,900,100);
                g.fillRect(0,0,100,800);
                if(!play&&!loose2&&!loose){
                    g.drawString("Snake", 430, 320);
                    g.drawString("Press Enter to Start", 400, 340);
                    g.drawString("Press M to play muliplayer", 375, 360);

                }
                if(pause){
                    g.drawString("Pause", 430, 320);
                               }
                if(loose||loose2){
                    if(loose){
                        if(multi) {
                            g.drawString("Player 2 Win ", 400, 320);
                            g.drawString("Your score: " + score2, 400, 340);
                        }
                        g.drawString("Player 1 Lose ",400,360);
                        g.drawString("Your score: "+score,400,380);
                    }
                    if(loose2){
                        g.drawString("Player 1 Win ",400,320);
                        g.drawString("Your score: "+score,400,340);
                        g.drawString("Player 2 Lose ",400,360);
                        g.drawString("Your score: "+score2,400,380);

                    }
                    g.drawString("Press R to Restart ",400,400);

                }
                g.setColor(Color.white);
                if(scoreinfo){
                    g.drawString("--TOP--",20,200);
                    g.drawString("1: "+t1,5,220);
                    g.drawString("2: "+t2,5,240);
                    g.drawString("3: "+t3,5,260);

                }
                g.drawString("---Menu---",15,20);
                g.drawString("Press: ",5,40);
                g.drawString("ENTER -> Start",5,60);
                g.drawString("R -> Reset",5,80);
                g.drawString("P -> Pause",5,100);
                g.drawString("T -> Save",5,120);
                g.drawString("Y -> Load",5,140);
                g.drawString("M -> Multiplayer",5,160);
                g.drawString("B -> Best score ",5,180);
                g.drawString("Score-1: "+score,700,825);
                if(multi) {
                    g.drawString("Score-2: " + score2, 700, 845);
                }


                 if((eat+eat2)%5==0||(eat+eat2)%6==0){
                         g5.drawImage(imgwall, wallx, wally, null);
                   }
                 if(play){
                if(((eat+eat2)%4==0)&&boost==1&&boost2==1){
                    g4.drawImage(imgfood2, foodx, foody, null);
                    boostpower=1;
                }
                else if(((eat+eat2)%7==0)){
                         g6.drawImage(imgfrog, foodx, foody, null);
                         frog=true;

                     }

                else {
                    g3.drawImage(imgfood, foodx, foody, null);
                    boostpower = 0;
                }
                }

                for (int i = 0; i < eat; i++) {
                    if(i==0){
                        g2.drawImage(imghead, snakex[i], snakey[i], null);
                    }
                    else {
                        ((Graphics2D) g).drawImage(imgbody, snakex[i], snakey[i], null);
                    }

                }
            if(multi) {
                for (int i = 0; i < eat2; i++) {
                    if(i==0) {
                        g4.drawImage(imghead2, snakex2[i], snakey2[i], null);
                    }
                    else {
                        ((Graphics2D) g).drawImage(imgbody, snakex2[i], snakey2[i], null);
                    }
                }
                }


            }

        public void actionPerformed(ActionEvent e){

                if(!play){
                    snakex[0]=600;
                    snakey[0]=600;
                    snakex[1]=630;
                    snakey[1]=600;
                    snakex2[0]=200;
                    snakey2[0]=200;
                    snakex2[1]=170;
                    snakey2[1]=200;

               }
            if(!loose&&!loose2&&play&&!pause) {

                if(downn||upp){
                    for(int i=eat-1;i>=0;i--){

                        snakex[i+1]=snakex[i];
                    }
                    for(int i=eat;i>=0;i--){
                        if(i==0){
                            snakey[i]=snakey[i]+25*vely;
                        }
                        else{
                            snakey[i]=snakey[i-1];
                        }
                    }
                }

                if(leftt||rightt){
                    for(int i=eat-1;i>=0;i--){

                        snakey[i+1]=snakey[i];
                    }
                    for(int i=eat;i>=0;i--){
                        if(i==0){
                            snakex[i]=snakex[i]+25*velx;

                        }
                        else{
                            snakex[i]=snakex[i-1];
                        }
                    }
                }
                if(multi) {
                    if (downn2 || upp2) {
                        for (int i = eat2 - 1; i >= 0; i--) {

                            snakex2[i + 1] = snakex2[i];
                        }
                        for (int i = eat2; i >= 0; i--) {
                            if (i == 0) {
                                snakey2[i] = snakey2[i] + 25 * vely2;
                            } else {
                                snakey2[i] = snakey2[i - 1];
                            }
                        }
                    }

                    if (leftt2 || rightt2) {
                        for (int i = eat2 - 1; i >= 0; i--) {

                            snakey2[i + 1] = snakey2[i];
                        }
                        for (int i = eat2; i >= 0; i--) {
                            if (i == 0) {
                                snakex2[i] = snakex2[i] + 25 * velx2;

                            } else {
                                snakex2[i] = snakex2[i - 1];
                            }
                        }
                    }
                }

            }

            if(!pause) {
                if(frog){
                    if (foodx < 890 && move == 1) {
                        foodx += 10;
                        if (foodx == 870) {
                            Random random = new Random();
                            foodx = random.nextInt((max-50) - (min+50) + 1) + (min+50);
                            foody = 0;
                        }
                    }
                    if (foody < 790 && move == 1) {
                        foody += 10;
                        if (foody == 770) {
                            Random random = new Random();
                            foody = random.nextInt((max-50) - (min+50) + 1) + (min+50);
                            foodx = 100;
                        }
                    }
                }
                if ((eat + eat2) % 5 == 0 || (eat + eat2) % 6 == 0) {
                    if (wallx < 890 && move == 1) {
                        wallx += 10;
                        if (wallx == 870) {
                            wallx = 100;
                        }
                    }
                } else {
                    wallx = 900;
                    wally = 900;

                }
            }

            if(snakex[0]<foodx+25&&snakex[0]>foodx-25&&snakey[0]<foody+25&&snakey[0]>foody-25){
                if(frog){
                    score += 20;
                    frog=false;
                }
                else if(boostpower==1){
                    boost = 2;
                    Random random = new Random();
                    wallx = random.nextInt((max-50) - (min+50) + 1) + (min+50);
                    wally = random.nextInt((max-50) - (min+50) + 1) + (min+50);
                    score += 10;
                }
                else{
                    boost=1;
                    score += 5;
                }
                Random random = new Random();
               foodx = random.nextInt((max-50) - (min+50) + 1) + (min+50);
               eat+=1;
               foody = random.nextInt((max-50) - (min+50) + 1) + (min+50);
            }


            if(multi) {
            if(snakex2[0]<foodx+25&&snakex2[0]>foodx-25&&snakey2[0]<foody+25&&snakey2[0]>foody-25){
                if(frog){
                    score2 += 20;
                }
                else if(boostpower==1){
                        boost2 = 2;
                    Random random = new Random();
                    wallx = random.nextInt((max-50) - (min+50) + 1) + (min+50);
                    wally = random.nextInt((max-50) - (min+50) + 1) + (min+50);
                    score2 += 10;
                }
                else{
                    boost2=1;
                    score2 += 5;
                }
                Random random = new Random();
                foodx = random.nextInt((max-50) - (min+50) + 1) + (min+50);
                eat2+=1;
                foody = random.nextInt((max-50) - (min+50) + 1) + (min+50);
            }
            if(snakex[0]==snakex2[0]&&snakey[0]==snakey2[0]){
                snakex[0]=600;
                snakey[0]=600;
                snakex[1]=630;
                snakey[1]=600;
                snakex2[0]=200;
                snakey2[0]=200;
                snakex2[1]=170;
                snakey2[1]=200;

                boost2=1;
                boost=1;
                 velx = 0;
                 vely = 0;
                 velx2 = 0;
                 vely2 = 0;
                 top(score2);
                 top(score);
                eat2=2;
                eat=2;
                score2=0;
                score=0;
            }

                for(int i=1;i<eat2;i++){
                    if(snakex2[i]==snakex2[0]&&snakey2[i]==snakey2[0]) {
                        velx = 0;
                        vely = 0;
                        velx2 = 0;
                        vely2 = 0;
                        loose2 = true;
                        play = false;
                        eat2=2;
                        eat=2;
                        top(score2);
                        top(score);
                    }
                    else if(snakex[i]==snakex2[0]&&snakey[i]==snakey2[0]) {
                        velx = 0;
                        vely = 0;
                        velx2 = 0;
                        vely2 = 0;
                        loose2 = true;
                        play = false;
                        eat2=2;
                        eat=2;
                        top(score2);
                        top(score);
                    }
                    else if(snakex2[i]==snakex[0]&&snakey2[i]==snakey[0]) {
                        velx = 0;
                        vely = 0;
                        velx2 = 0;
                        vely2 = 0;
                        loose = true;
                        play = false;
                        eat2=2;
                        eat=2;
                        top(score2);
                        top(score);
                    }




                }

            if(snakex2[0]>845||snakey2[0]>770||snakex2[0]<100||snakey2[0]<0||(snakex2[0]<wallx+100&&snakex2[0]>wallx-30&&snakey2[0]<wally+20&&snakey2[0]>wally-20)){
                velx = 0;
                vely = 0;
                velx2 = 0;
                vely2 = 0;
                loose2 = true;
                play = false;
                eat2=2;
                eat=2;
                top(score2);
                top(score);
            }
            }

            for(int i=1;i<eat;i++){
                if(snakex[i]==snakex[0]&&snakey[i]==snakey[0]) {

                    velx = 0;
                    vely = 0;
                    velx2 = 0;
                    vely2 = 0;
                    eat2=2;
                    eat=2;
                    loose = true;
                    play = false;
                    top(score2);
                    top(score);
                }
            }
            System.out.println(snakex[0]);
            if(snakex[0]>845||snakey[0]>770||snakex[0]<100||snakey[0]<0||(snakex[0]<wallx+100&&snakex[0]>wallx-30&&snakey[0]<wally+20&&snakey[0]>wally-20)){
                velx = 0;
                vely = 0;
                velx2 = 0;
                vely2 = 0;
                eat2=2;
                eat=2;
                loose = true;
                play = false;
                top(score2);
                top(score);
            }
            repaint();
        }

        public void up(){
            velx = 0;
            vely = -1*boost;
        }
        public void down(){
            velx = 0;
            vely = 1*boost;
        }
        public void left(){
            vely = 0;
            velx = -1*boost;
        }
        public void right(){
            vely = 0;
            velx = 1*boost;
        }

        public void up2(){
            velx2 = 0;
            vely2 = -1*boost2;
        }
        public void down2(){
            velx2 = 0;
            vely2 = 1*boost2;
        }
        public void left2(){
            vely2 = 0;
            velx2 = -1*boost2;
        }
        public void right2(){
            vely2 = 0;
            velx2 = 1*boost2;
        }


        public void keyPressed(KeyEvent e){
            int c = e.getKeyCode();
            if(c == KeyEvent.VK_UP){
                upp = true;
                if(!downn) {
                    up();
                    upp = true;}
                else{
                    downn = true;
                    upp=false;
                }
                leftt=false;
                rightt=false;
            }
            if(c == KeyEvent.VK_DOWN){
                downn = true;
                if(!upp) {
                    down();
                    downn = true;}
                else{
                    upp = true;
                    downn=false;
                }
                leftt=false;
                rightt=false;
            }
            if(c == KeyEvent.VK_LEFT){
                leftt = true;
                upp=false;
                downn=false;
                if(!rightt) {
                    left();
                    leftt = true;}
                else{
                    rightt = true;
                    leftt=false;
                }
            }
            if(c == KeyEvent.VK_RIGHT){
                rightt = true;
                upp=false;
                downn=false;
                if(!leftt) {
                    right();
                    rightt = true;}
                else{
                    leftt = true;
                    rightt=false;
                }
            }

            if(multi) {
                if (c == KeyEvent.VK_W) {
                    upp2 = true;
                    if(!downn2) {
                        up2();
                        upp2 = true;}
                    else{
                        downn2 = true;
                        upp2=false;
                    }
                    leftt2=false;
                    rightt2=false;

                }
                if (c == KeyEvent.VK_S) {
                    downn2 = true;
                    if(!upp2) {
                        down2();
                        downn2 = true;}
                    else{
                        upp2 = true;
                        downn2=false;
                    }
                    leftt2=false;
                    rightt2=false;
                }
                if (c == KeyEvent.VK_A) {
                    leftt2 = true;
                    upp2=false;
                    downn2=false;
                    if(!rightt2) {
                        left2();
                        leftt2 = true;}
                    else{
                        rightt2 = true;
                        leftt2=false;
                    }
                }
                if (c == KeyEvent.VK_D) {
                    rightt2 = true;
                    upp2=false;
                    downn2=false;
                    rightt2=true;
                    if(!leftt2) {
                        right2();
                        rightt2 = true;}
                    else{
                        leftt2 = true;
                        rightt2=false;
                    }
                }
            }

            if(c == KeyEvent.VK_T){
                if(pause) {
                    savegame();
                }

            }

            if(c == KeyEvent.VK_Y){
                if(pause||!play) {
                    loadgame();
                    loadg=true;
                }

            }

            if(c == KeyEvent.VK_P){
                if(pause) {
                    pause = false;
                }
                else {
                    pause = true;
                }

            }

            if(c == KeyEvent.VK_ENTER){
                if((velx==0&&vely==0&&velx2==0&&vely2==0)||loadg) {
                    if(!loadg){
                        Random random = new Random();
                    foodx = random.nextInt((max-50) - (min+50) + 1) + (min+50);
                    foody = random.nextInt((max-50) - (min+50) + 1) + (min+50);
                    leftt=true;
                    left();
                    if(multi) {
                        rightt2=true;
                        right2();
                    }}
                    play = true;
                }
                loadg=false;
            }
            if(c == KeyEvent.VK_M){
                if(multi&&velx==0&&vely==0&&velx2==0&&vely2==0) {
                    multi = false;
                }
                else if(!multi&&velx==0&&vely==0&&velx2==0&&vely2==0){
                    multi = true;
                }
            }
            if(c == KeyEvent.VK_B){
                if(scoreinfo) {
                    scoreinfo = false;
                }
                else {
                    scoreinfo = true;
                    top(0);
                }
            }
            if(c == KeyEvent.VK_R){
                snakex[0]=600;
                snakey[0]=600;
                snakex[1]=630;
                snakey[1]=600;
                snakex2[0]=200;
                snakey2[0]=200;
                snakex2[1]=170;
                snakey2[1]=200;

                eat2=2;
                eat=2;
                boost2=1;
                boost=1;
                velx = 0;
                vely = 0;
                velx2 = 0;
                vely2 = 0;
                score=0;
                score2=0;
                loose = false;
                loose2 = false;
                pause=false;
                Random random = new Random();
                foodx = random.nextInt((max-50) - (min+50) + 1) + (min+50);
                foody = random.nextInt((max-50) - (min+50) + 1) + (min+50);

            }

        }

    public void keyTyped(KeyEvent e) {}
    public void keyReleased(KeyEvent e) {}

        public static void writeFile(String Filename, String text,String text2,String text3)
                throws IOException
        {
            File file = new File (Filename);
            BufferedWriter out = new BufferedWriter(new FileWriter(file));
            out.write(text);
            out.newLine();
            out.write(text2);
            out.newLine();
            out.write(text3);
            out.newLine();
            out.close();
        }

        public static void readFile(String Filename)
                throws Exception
        {

            File file = new File (Filename);

            BufferedReader br = new BufferedReader(new FileReader(file));

            t1=Integer.parseInt(br.readLine());
            t2=Integer.parseInt(br.readLine());
            t3=Integer.parseInt(br.readLine());
            br.close();
        }

        public static void save(String Filename, String p1,String p2,String p3,String p4,String p5,String p6)
                throws IOException
        {
            File file = new File (Filename);
            BufferedWriter out = new BufferedWriter(new FileWriter(file));
            out.write(p1);
            out.newLine();
            out.write(p2);
            out.newLine();
            out.write(p3);
            out.newLine();
            out.write(p4);
            out.newLine();
            out.write(p5);
            out.newLine();
            out.write(p6);
            out.newLine();
            out.close();
        }

        public static void load(String Filename)
                throws Exception
        {

            File file = new File (Filename);

            BufferedReader br = new BufferedReader(new FileReader(file));

            p1=Integer.parseInt(br.readLine());
            p2=Integer.parseInt(br.readLine());
            p3=Integer.parseInt(br.readLine());
            p4=Integer.parseInt(br.readLine());
            p5=Integer.parseInt(br.readLine());
            p6=Integer.parseInt(br.readLine());
            br.close();
        }

        public void loadgame(){
            try {
                load("foods.txt");
                foodx=p1;
                foody=p2;
                load("player1s.txt");
                snakex[0]=p1;
                snakey[0]=p2;
                velx=p3;
                vely=p4;
                eat=p5;
                score=p6;
                if(multi){
                    load("foodm.txt");
                    foodx=p1;
                    foody=p2;
                    load("player1m.txt");
                    snakex[0]=p1;
                    snakey[0]=p2;
                    velx=p3;
                    vely=p4;
                    eat=p5;
                    score=p6;
                load("player2.txt");
                snakex2[0]=p1;
                snakey2[0]=p2;
                velx2=p3;
                vely2=p4;
                eat2=p5;
                score2=p6;}
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
        public void savegame(){
            try {
                save("player1s.txt",Integer.toString(snakex[0]),Integer.toString(snakey[0]),Integer.toString(velx),Integer.toString(vely),Integer.toString(eat), Integer.toString(score2));
                save("foods.txt",Integer.toString(foodx),Integer.toString(foody),Integer.toString(0),Integer.toString(0),Integer.toString(0), Integer.toString(0));
                if(multi) {
                    save("player1m.txt",Integer.toString(snakex[0]),Integer.toString(snakey[0]),Integer.toString(velx),Integer.toString(vely),Integer.toString(eat), Integer.toString(score2));
                    save("foodm.txt",Integer.toString(foodx),Integer.toString(foody),Integer.toString(0),Integer.toString(0),Integer.toString(0), Integer.toString(0));
                    save("player2.txt", Integer.toString(snakex2[0]), Integer.toString(snakey2[0]), Integer.toString(velx2), Integer.toString(vely2), Integer.toString(eat2), Integer.toString(score2));
                }
            } catch (Exception e) {
                e.printStackTrace();
            }
        }




        public void top(int wynik){

            try {
                readFile("top.txt");
            } catch (Exception e) {
                e.printStackTrace();
            }
            if(wynik>t1){
                t1=wynik;
            }
            else if(wynik>t2){
                t2=wynik;
            }
            else if(wynik>t3){
                t3=wynik;
            }

            try {
                writeFile("top.txt", Integer.toString(t1), Integer.toString(t2), Integer.toString(t3));
            } catch (IOException e) {
                e.printStackTrace();
            }



        }


    }

