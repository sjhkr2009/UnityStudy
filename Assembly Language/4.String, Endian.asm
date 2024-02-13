%include "io64.inc"

section .text
global main
main:
    ; 참고) 지난 파일에 적었던 레지스터 종류
    ; 32비트: eax(연산), ebx(메모리 주소), ecx(반복횟수), edx(데이터 저장)
    ; 64비트 버전: rax, rbx, rcx, rdx
    ; 16비트 버전: ax, bx, cx, dx
    ;  ㄴ 각각 8비트의 (ah,al), (bh,bl), (ch,cl), (dh,dl) 로 구성
    
    ; 참고로 PRINT_STRING은 어셈블리어의 기능은 아니고, 운영체제에 문자열 출력을 요청하는 SASM의 기능
    PRINT_STRING msg

    xor rax, rax
    ret
    
section .data
    ; db(1바이트), dw(2바이트), dd(3바이트), dq(8바이트)
    a db 0x11, 0x11, 0x11, 0x14 ; 변수에 여러 개의 값을 넣을수도 있다. 배열처럼 메모리의 옆 칸에 들어간다.
    
    ; 문자열을 사용하려면 변수에 char 배열을 넣고, 마지막에는 문자열의 끝을 알리는 0x00 값을 넣는다.
    msg db 'Hello, World!', 0x00 ; 0x48, 0x65, 0x6c, 0x6f, ..., 0x00 을 넣는것과 동일함
    
    ; 엔디안: 메모리에서 데이터를 나열하는 방식. 서로 다른 환경 사이의 통신에서 엔디안 이슈가 발생할 수 있음.
    ; - 빅 엔디안: 메모리에 데이터가 낮은 주소에서 높은 주소 순으로 할당 (Java 가상환경 등)
    ;  ㄴ 장점: 직관적이고, 숫자 대소관계 비교 시 앞쪽부터 비교하면 된다
    ; - 리틀 엔디안: 메모리에 데이터가 높은 주소에서 낮은 주소 순으로 할당 (Intel/AMD, 대부분의 데스크탑 환경)
    ;  ㄴ 장점: 캐스팅 시 첫 번째 주소만 읽으면 된다
    
    
section .bss
    ; resb(1바이트), resw(2), resd(4), resq(8)
    e resb 5
    