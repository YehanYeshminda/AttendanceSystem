import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';

@Component({
  selector: 'app-input-form',
  templateUrl: './input-form.component.html',
  styleUrls: ['./input-form.component.scss'],
})
export class InputFormComponent implements ControlValueAccessor {
  @Input() label = '';
  @Input() type = 'text';
  @Input() lengthOfInput: string = '100%';
  @Input() haveLabel: boolean = true;
  @Input() readOnly: boolean = false;

  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
  }

  writeValue(obj: any): void {}

  registerOnChange(fn: any): void {}

  registerOnTouched(fn: any): void {}

  get control(): FormControl {
    return this.ngControl.control as FormControl;
  }
}
