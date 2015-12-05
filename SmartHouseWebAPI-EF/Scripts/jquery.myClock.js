// Немедленно вызываемая функция в JavaScript (IIFE) — это синтаксическая конструкция,
// позволяющая вызвать функцию сразу же в месте ее определения.
//
// That's a common technique to assure you have a true undefined value to check against since the 
// undefined property of the window object used to be writeable and thus could not reliably be used in checks. 
// Since only one parameter is handed over to the function, the second one is assured to be undefined.
(function($, undefined) {
	//alert($);
	// Вывод: function (a,b){return new n.fn.init(a,b)}

	// $.fn представляет собой псевдоним JavaScript-свойства jQuery.prototype. Две следующие строки эквивалентны.
	// $.prototype.testFunc = function(options) {
	$.fn.myClock = function(options) {
		// Зададим список свойств и укажем для них значения по умолчанию. Если при вызове метода будут указаны пользовательские
		// варианты некоторых из них, то они автоматически перепишут соответствующие значения по умолчанию.
		options = $.extend({
			timestamp: 0,
			disabled: false
		}, options);
		
		var executingFunction = function() {
			// Внимание! установка CSS класса, возможно надо будет менять!!!
			if (!$(this).hasClass("jqclock")){
				$(this).addClass("jqclock");
			}
			
			if (options.disabled == true)
			{
				$(this).html(" <span class='clocktime'>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp        </span>");
				return;
			}
			
			// В рамках этой задачи нужно просто отобразить время, которое приходит от сервера.
			options.timestamp += new Date(options.timestamp).getTimezoneOffset() * 60 * 1000;
						
			var sysTimeStamp = new Date().getTime();
			var myTimeStamp = new Date(options.timestamp).getTime();
			var delta = sysTimeStamp - myTimeStamp;
						
			clockInfoManager.registerNewClock(this, delta);
			var time = new Date(sysTimeStamp - delta);
			updateClock(this, time);
		}
				
		// Переменная this, в теле метода, всегда содержит текущий объект jQuery (тот, на котором и был вызван метод a 
		// не саму функцию jQuery. Другими словами он содержит объект выборки). alert(this.selector);
		// Для того, чтобы обойти все выбранные элементы по отдельности, используем метод .each(). А для возможности 
		// продолжить цепочку методов наш метод должен будет возвратить текущий объект jQuery.
		return this.each(executingFunction);
	}
	
	function updateClock(el, time){
        var h = time.getHours();
        var m = time.getMinutes();
        var s = time.getSeconds();

        // add a zero in front of numbers 0-9
        h = addLeadingZero(h);
        m = addLeadingZero(m);
        s = addLeadingZero(s);

        $(el).html(" <span class='clocktime'>" + h + ":" + 
			m + ":" + s + "</span>");
    }

	function addLeadingZero(number){
      if (number < 10){number = "0" + number;}
      return number;
    }	
	
	function clockInfo (element, delta){
		this._element = element;
		this._delta = delta;
	}
	
	var clockInfoManager = {
		// Properties
		clInfArray: [],
		
		// Methods
		registerNewClock: function(element, delta){
			var elemFound = this.findElement(element);
			
			if (elemFound == null){
				var ci = new clockInfo(element, delta);
				this.clInfArray.push(ci);
			}
			else{
				elemFound._delta = delta;
			}			
		},
		findElement: function(element){
			var arr = this.clInfArray;
			var returnElem = null;
			for (var i = 0; i < arr.length; i++)
			{
				if (arr[i]._element == element){
					returnElem = arr[i];
					break;					
				}
			}
			return returnElem;
		},
		timerHanlder: function(){
			var arr = this.clInfArray;
			var sysTimeStamp = new Date().getTime();
			for (var i = 0; i < arr.length; i++){
				var time = new Date(sysTimeStamp - arr[i]._delta);
				updateClock(arr[i]._element, time);
			}
		},
		timer: setInterval(function(){ clockInfoManager.timerHanlder(); } , 1000)
	}
	
	// В качестве параметра паредаётся функция jQuery. Попадает она в параметр по имени $,
	// эта переменная является локальной и таким образом не будет конфликтовать с другими переменными $ из глобальной области видимости,
	// которые могут использоваться в других библиотеках.
})(jQuery);