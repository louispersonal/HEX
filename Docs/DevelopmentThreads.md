\# Development Threads



\## Active



\### Selection architecture

Status: Next



\- \[ ] Primary selection

\- \[ ] Selected hex, region, and pop context

\- \[ ] Primary-pop highlight



\### Player movement UI

Status: Next



\- \[ ] Only one MoveJob per pop

\- \[ ] Explicit destination-selection mode

\- \[ ] Escape/right-click cancellation

\- \[ ] Display committed movement path



\### Pop view culling

Status: Parked



\- \[ ] Only instantiate/activate PopViews near the camera viewport

\- \[ ] React when a pop enters or leaves the rendered area

\- \[ ] Preserve PopID-based view ownership



\### Fractional movement costs

Decision needed: integer tick costs or accumulated float progress?



\## Completed



\- \[x] Move job progression from Brain to Pawn

\- \[x] Add job tick phase

\- \[x] Introduce movement cost providers

\- \[x] Store pops by stable PopID

\- \[x] Add secondary spatial index

\- \[x] Require location in Pop constructor

\- \[x] Remove Teleport

\- \[x] Keep PopView alive while moving

\- \[x] Fail MoveJob when a step cannot execute



