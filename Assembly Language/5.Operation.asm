%include "io64.inc"

section .text
global main
main:
    mov rbp, rsp; for correct debugging
    
    ; 사칙연산
    ; 참고로 어셈블리어에서 대소문자는 무관함
    
    ; 참고) 지난 파일에 적었던 레지스터 종류
    ; 32비트: eax(연산), ebx(메모리 주소), ecx(반복횟수), edx(데이터 저장)
    ; 64비트 버전: rax, rbx, rcx, rdx
    ; 16비트 버전: ax, bx, cx, dx
    ;  ㄴ 각각 8비트의 (ah,al), (bh,bl), (ch,cl), (dh,dl) 로 구성
    
    ; GET_DEC: Input으로 들어오는 십진수 하나를 입력받는다. 
    ; 이것도 PRINT_STRING처럼 어셈블리어는 아니고, SASM이 운영체제 API를 호출해주는 헬퍼 매크로
    GET_DEC 1, al ; 오른쪽 Input란에 입력한 숫자가 al 영역에 들어온다.
    GET_DEC 1, num
    
    PRINT_DEC 1, al ; al의 정수를 Output에 출력한다
    NEWLINE
    PRINT_DEC 1, num
    NEWLINE
    
    ; 더하기 연산
    ; add a, b : a+b를 a에 넣는다 (프로그래밍 언어의 a = a + b 와 동일한 동작)
    ; a는 레지스터 또는 메모리만, b는 레지스터/메모리/상수 모두 가능.
    ; 단, a,b 모두 메모리면 안 된다
    add al, 1 ; al에 1을 더해서 al에 넣는다. (레지스터 + 상수)
    
    PRINT_DEC 1, al ; al이 받은 input값에 1이 더해진 값이 출력됨 (1+1=2)
    NEWLINE
    
    ; num은 메모리 주소일 뿐 메모리에 담긴 값이 아님에 주의. 이전 시간 '변수' 파트 참고.
;    add al, num ; 에러. 메모리 주소값을 1바이트짜리 al에 더하려고 해서 크래시가 발생함.
    add al, [num] ; num 메모리의 값을 al에 더한다 (레지스터 + 메모리)
    
    PRINT_DEC 1, al ; 2+2=4
    NEWLINE
    
    mov bl, 3 ; (레지스터 + 레지스터)
    add al, bl
    PRINT_DEC 1, al ; 4+3=7
    NEWLINE
    
    ; 메모리 주소에 연산을 할 때는 크기를 명시해야 한다.
    add [num], byte 1 ; (메모리 + 상수)
    PRINT_DEC 1, [num] ; 2+1=3
    NEWLINE
    
    add [num], al; (메모리 + 레지스터)
    PRINT_DEC 1, [num] ; 3+7=10
    NEWLINE
    
    ; 메모리끼리의 연산은 불가능
    ;add [num], [num] 
    
    ; 빼기 연산: sub a, b (프로그래밍 언어의 a = a - b 와 동일한 동작)
    
    ; 곱하기 연산: mul r
    ; r 자리는 레지스터만 가능
    ; a 레지스터는 곱하기 연산을 위해 항상 예약되어 있다.
    ; 연산 결과는 a 레지스터(+d 레지스터)의 더 큰 공간에 저장한다. 즉,
    ;  - 'mul bl' 은 al * bl 값을 ax에 저장한다.
    ;  - 'mul bx' 는 ax * bx 값을 dx(상위 16bit), ax(하위 16bit)에 나눠서 저장한다
    
    ; 여기서는 1바이트 (al,bl) 곱하기 연산만 하기로 한다.
    ; ax를 초기화하고, al에 5를 넣어두고, bl에 8을 넣은 후 mul bl을 호출 -> 결과가 저장될 ax 출력
    mov ax, 0
    mov al, 5
    mov bl, 8
    mul bl
    PRINT_DEC 2, ax ; 5 * 8 = 40
    NEWLINE
    
    ; 나누기 연산: div r
    ; 곱하기처럼 a 레지스터의 값이 예약되어 있으며, 입력된 값을 나눠 하위 공간에 저장한다. 즉,
    ;  - 'div bl'은 ax / bl 을 수행하고, al에 몫을, ah에 나머지를 저장한다.
    mov ax, 100
    mov bl, 3
    div bl
    PRINT_DEC 1, al ; 100/3의 몫 = 33
    NEWLINE
    mov al, ah ; PRINT_DEC는 ah를 출력할 수 없으므로, al에 ah값을 넣은 후 출력한다
    PRINT_DEC 1, al ; 100/3의 나머지 = 1
    NEWLINE
    
    xor rax, rax
    ret
    
section .data
    ; 초기화된 데이터 - {변수명} {크기} {초기값}
    ; 크기: db(1바이트), dw(2바이트), dd(3바이트), dq(8바이트)
    
section .bss
    ; 초기화되지 않은 데이터 - {변수명} {크기} {개수}
    ; 크기: resb(1바이트), resw(2), resd(4), resq(8)
    num resb 1
    