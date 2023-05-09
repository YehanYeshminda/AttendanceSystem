import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';

@Component({
  selector: 'app-typeahead-form',
  templateUrl: './typeahead-form.component.html',
  styleUrls: ['./typeahead-form.component.scss'],
})
export class TypeaheadFormComponent implements ControlValueAccessor {
  @Input() data: any[] = [];
  @Input() label = '';

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
