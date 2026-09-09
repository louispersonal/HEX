\# Development Threads



\## Active



\### Pop movement architecture

Status: In progress



\- \[ ] Store pops by stable PopID

\- \[ ] Add secondary spatial index

\- \[ ] Require location in Pop constructor

\- \[ ] Remove Teleport

\- \[ ] Keep PopView alive while moving

\- \[ ] Fail MoveJob when a step cannot execute



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

