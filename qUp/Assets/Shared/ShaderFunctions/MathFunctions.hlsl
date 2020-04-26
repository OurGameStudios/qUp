void AdvancedSine_float (float T, 
     float OffsetX,
     float Min,
     float Max,
     float DelayMidMax,
     float MidMax,
     float DelayMax,
     float MaxMid,
     float DelayMidMin,
     float MidMin,
     float DelayMin,
     float MinMid,
     float DelayAfter,
     out float Result)
 {
     float pi = asin(1) * 2;
     //Sine keyframes
     float key1 = DelayMidMax;
     float key2 = key1 + MidMax;
     float key3 = key2 + DelayMax;
     float key4 = key3 + MaxMid;
     float key5 = key4 + DelayMidMin;
     float key6 = key5 + MidMin;
     float key7 = key6 + DelayMin;
     float key8 = key7 + MinMid;
     float key9 = key8 + DelayAfter;
     
     float amp = abs(Max - Min);
     float t = (T + OffsetX) % (key8);
     
     if (t < key1) {
         Result = (sin(0)/2 + 0.5) * amp + Min;
     } else if (t < key2) {
         float x = lerp(0, pi/2, (t-key1)/(key2-key1));
         Result = (sin(x)/2 + 0.5) * amp + Min;
     } else if (t < key3) {
         Result = (sin(pi/2)/2 + 0.5) * amp + Min;
     } else if (t < key4) {
         float x = lerp(pi/2, pi, (t-key3)/(key4-key3));
         Result = (sin(x)/2 + 0.5) * amp + Min;
     } else if (t < key5) {
         Result = (sin(pi)/2 + 0.5) * amp + Min;
     } else if (t < key6) {
         float x = lerp(pi, 3*pi/2, (t-key5)/(key6-key5));
         Result = (sin(x)/2 + 0.5) * amp + Min;
     } else if (t < key7) {
         Result = (sin(3*pi/2)/2 + 0.5) * amp + Min;
     } else if (t < key8) {
         float x = lerp(3*pi/2, 2*pi, (t-key7)/(key8-key7));
         Result = (sin(x)/2 + 0.5) * amp + Min;
     } else if (t < key9) {
         Result = (sin(2*pi)/2 + 0.5) * amp + Min;
     } else {
         Result = (sin(2*pi)/2 + 0.5) * amp + Min;
     }
 }
 
 void AdvancedCosine_float (float T, 
    float OffsetX,
    float Min,
    float Max,
    float DelayMin,
    float MinMid,
    float DelayMidMax,
    float MidMax,
    float DelayMax,
    float MaxMid,
    float DelayMidMin,
    float MidMin,
    float DelayAfter,
    out float Result)
{
    float pi = asin(1) * 2;
    //Sine keyframes
    float key1 = DelayMin;
    float key2 = key1 + MinMid;
    float key3 = key2 + DelayMidMax;
    float key4 = key3 + MidMax;
    float key5 = key4 + DelayMax;
    float key6 = key5 + MaxMid;
    float key7 = key6 + DelayMidMin;
    float key8 = key7 + MidMin;
    float key9 = key8 + DelayAfter;
    
    float amp = abs(Max - Min);
    float t = (T + OffsetX) % (key8);
    
    if (t < key1) {
        Result = (cos(0)/2 + 0.5) * amp + Min;
    } else if (t < key2) {
        float x = lerp(0, pi/2, (t-key1)/(key2-key1));
        Result = (cos(x)/2 + 0.5) * amp + Min;
    } else if (t < key3) {
        Result = (cos(pi/2)/2 + 0.5) * amp + Min;
    } else if (t < key4) {
        float x = lerp(pi/2, pi, (t-key3)/(key4-key3));
        Result = (cos(x)/2 + 0.5) * amp + Min;
    } else if (t < key5) {
        Result = (cos(pi)/2 + 0.5) * amp + Min;
    } else if (t < key6) {
        float x = lerp(pi, 3*pi/2, (t-key5)/(key6-key5));
        Result = (cos(x)/2 + 0.5) * amp + Min;
    } else if (t < key7) {
        Result = (cos(3*pi/2)/2 + 0.5) * amp + Min;
    } else if (t < key8) {
        float x = lerp(3*pi/2, 2*pi, (t-key7)/(key8-key7));
        Result = (cos(x)/2 + 0.5) * amp + Min;
    } else if (t < key9) {
        Result = (cos(2*pi)/2 + 0.5) * amp + Min;
    } else {
        Result = (cos(2*pi)/2 + 0.5) * amp + Min;
    }
}