set terminal postscript eps color
#set key inside right top vertical Right noreverse enhanced autotitles box linetype -1 linewidth 0.200
set key outside right top vertical Right noreverse enhanced autotitles box linetype -1 linewidth 0.200
set bars small
#set size 0.5,0.5

set terminal svg size 1400,1100 font "Helvetica,24"
set pal gray
set key autotitle columnhead
set datafile separator "\t"

set termoption dash

set xtics in scale 0.3
set ytics in scale 0.3

set dashtype 2 (6.0,2.0)
set dashtype 3 (2.0,2.0)

set linestyle 1 lt 1 lw 2 lc rgb "#000000" dashtype 1
set linestyle 2 lt 2 lw 2 lc rgb "#000000" dashtype 2
set linestyle 3 lt 3 lw 2 lc rgb "#000000" dashtype 3
set linestyle 4 lt 4 lw 2 lc rgb "#909090" dashtype 1
set linestyle 5 lt 5 lw 2 lc rgb "#909090" dashtype 2
set linestyle 6 lt 7 lw 2 lc rgb "#909090" dashtype 3
set linestyle 7 lt 1 lw 2 lc rgb "#909090" dashtype 1

#set linestyle 1 lt 1 lw 3 lc rgb "red" dashtype 1
#set linestyle 2 lt 2 lw 3 lc rgb "blue" dashtype 1
#set linestyle 3 lt 3 lw 3 lc rgb "green" dashtype 1
#set linestyle 4 lt 4 lw 3 dashtype 1
#set linestyle 5 lt 5 lw 3 dashtype 1
#set linestyle 6 lt 6 lw 3 dashtype 1
#set linestyle 7 lt 7 lw 3 lc rgb "magenta" dashtype 1
#set linestyle 8 lt 8 lw 3 dashtype 1


#set y2label "E, kJ/mol"
#set autoscale y2
#set y2tics
#set ytics nomirror

set key samplen 1.5

set key box opaque
set style textbox opaque

unset key
unset label
set autoscale x
set autoscale y
#set yrange [-50.0:250]
#set xrange [1.0:4.5]
set output "Lens.outGradient-Ni.svg"
set ylabel "E, ГэВ"
set xlabel "Y, mm"
plot "Lens.outGradient-Ni.csv" using 4:($5/1000000.0) with lines linestyle 1

unset key
unset label
set autoscale x
set autoscale y
#set yrange [-50.0:250]
#set xrange [1.0:4.5]
set output "Lens.outGradient-Al.svg"
set ylabel "E, ГэВ"
set xlabel "Y, mm"
plot "Lens.outGradient-Al.csv" using 4:($5/1000000.0) with lines linestyle 1

unset key
unset label
set autoscale x
set autoscale y
#set yrange [-50.0:250]
#set xrange [1.0:4.5]
set output "Lens.outGradient-Au.svg"
set ylabel "E, ГэВ"
set xlabel "Y, mm"
plot "Lens.outGradient-Au.csv" using 4:($5/1000000.0) with lines linestyle 1

quit
