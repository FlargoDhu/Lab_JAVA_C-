import javax.swing.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.Random;

class Main  {

    JFrame frame;
    JPanel panel;
    Timer timer;
    JButton click;

    public static void main(String[] args) {
        Main test = new Main();
        test.go();
    }

    public void go() {

        final int[] x = {200};
        final int[] y = {200};

        frame = new JFrame();
        panel = new JPanel();
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setSize(500, 500);

        click = new JButton("Click");

        click.addActionListener(new StartListener());

        click.setLocation(x[0], y[0]);
        click.setSize(70,30);
        panel.setSize(500,500);
        panel.setLayout(null);
        panel.add(click);
        frame.add(panel);

        frame.setVisible(true);
        click.setVisible(true);
        panel.setVisible(true);

        int max = 400;
        int min = 100;
        Random ran = new Random();
        int[] newx = {ran.nextInt((max - min) + 1) + min};
        int[] newy = new int[] {ran.nextInt((max - min) + 1) + min};

        final int[] stepx = {x[0] / newx[0]};
        final int[] stepy = {y[0] / newy[0]};
        if (stepx[0] == 0){
            stepx[0] = 1;
        }
        if (stepy[0] == 0){
            stepy[0] = 1;
        }

        timer = new Timer(10, new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent ae) {

                if(newx[0]>x[0]&&newy[0]>y[0]) {
                    x[0] += stepx[0];
                    y[0] += stepy[0];
                }
                else if(newx[0]<x[0]&&newy[0]>y[0]) {
                    x[0] -= stepx[0];
                    y[0] += stepy[0];
                }
                else if(newx[0]<x[0]&&newy[0]<y[0]) {
                    x[0] -= stepx[0];
                    y[0] -= stepy[0];
                }
                else if(newx[0]>x[0]&&newy[0]<y[0]) {
                    x[0] += stepx[0];
                    y[0] -= stepy[0];
                }
                else  {
                    timer.stop();
                    System.out.println ("cos");
                    newx[0] = ran.nextInt((max - min) + 1) + min;
                    newy[0] = ran.nextInt((max - min) + 1) + min;

                    stepx[0] = x[0]/newx[0];
                    stepy[0] = y[0]/newy[0];
                    if (stepx[0] == 0){
                        stepx[0] = 1;
                    }
                    if (stepy[0] == 0){
                        stepy[0] = 1;
                    }
                }
                click.setLocation(x[0], y[0]);
                frame.repaint();
            }
        });
    }

    class StartListener implements ActionListener {
        public void actionPerformed(ActionEvent e) {

            timer.start();
        }
    }


}