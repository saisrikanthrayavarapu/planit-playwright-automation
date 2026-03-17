package com.planit.utils;

import java.math.BigDecimal;
import java.math.RoundingMode;

public class PriceUtils {

    private PriceUtils() {
        // Utility class — no instantiation
    }

    /**
     * Rounds a double value to 2 decimal places using HALF_UP rounding.
     *
     * @param value the value to round
     * @return the value rounded to 2 decimal places
     */
    public static double round2dp(double value) {
        return BigDecimal.valueOf(value)
                .setScale(2, RoundingMode.HALF_UP)
                .doubleValue();
    }
}
