import javax.swing.*;
import java.awt.*;
import java.awt.Label;


public class Main {
    public static void main(String args[]){
        JFrame frame = new JFrame("Snake");
        snake s = new snake();
        frame.add(s);
        s.setSize(900,900);
        frame.setVisible(true);
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setSize(900,900);
    }
}
