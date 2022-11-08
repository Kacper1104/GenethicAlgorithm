package model;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;

@Builder
@AllArgsConstructor
public class City implements Comparable<City>{
    @Getter
    private Integer index;
    @Getter
    private Double X;
    @Getter
    private Double Y;

    public Double calculateDistance(City otherCity) {
        return Math.sqrt(Math.pow(this.X - otherCity.getX(), 2))  +
                Math.sqrt(Math.pow(this.Y - otherCity.getY(), 2));
    }

    public String toString() {
        return String.format(
                "%s %.2f %.2f ",
                this.index.toString(),
                this.X,
                this.Y
        );
    }

    @Override
    public int compareTo(City otherCity) {
        return Integer.compare(getIndex() ,otherCity.getIndex());
    }

    public City deepClone() {
        return City.builder()
                .index(getIndex())
                .X(getX())
                .Y(getY())
                .build();
    }
}
