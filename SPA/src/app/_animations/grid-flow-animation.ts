import {
  trigger,
  transition,
  query,
  style,
  stagger,
  keyframes,
  animate,
  AnimationTriggerMetadata
} from '@angular/animations';

export class GridFlowAnimation {
  static animations = GridFlowAnimation.getAnimation();
  static getAnimation(): Array<AnimationTriggerMetadata> {
    return [ trigger('bounce', [
      transition('* => *', [
        query(':enter', style({ opacity: 0 }), { optional: true }),
        // each time the binding value changes
        query(
          ':enter',
          [
            stagger('100ms', [
              animate(
                '.5s ease-in',
                keyframes([
                  style({
                    opacity: 0,
                    transform: 'translateY(-75px)',
                    offset: 0
                  }),
                  style({
                    opacity: 0.5,
                    transform: 'translateY(35px)',
                    offset: 0.3
                  }),
                  style({ opacity: 1, transform: 'translateY(0px)', offset: 1 })
                ])
              )
            ])
          ],
          { optional: true }
        ),
        query(
          ':leave',
          [
            stagger('30ms', [
              animate(
                '.4s ease-in',
                keyframes([
                  style({
                    opacity: 1,
                    transform: 'translateY(0px)',
                    offset: 0
                  }),
                  style({
                    opacity: 0.5,
                    transform: 'translateY(35px)',
                    offset: 0.3
                  }),
                  style({
                    opacity: 0,
                    transform: 'translateY(-75px)',
                    offset: 0.7
                  })
                ])
              )
            ])
          ],
          { optional: true }
        )
      ])
    ])];
  }
}
