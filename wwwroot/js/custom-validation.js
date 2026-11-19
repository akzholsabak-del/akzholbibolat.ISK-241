// [MinimumAge] атрибутының клиент жағындағы нұсқасы.
// Сервер HTML-ге data-val-minimumage және data-val-minimumage-years жазады,
// jquery.validate.unobtrusive оларды оқып, төмендегі ережені іске қосады.
(function ($) {
    $.validator.addMethod("minimumage", function (value, element, years) {
        if (!value) {
            return true; // бос болса, [Required] қарайды
        }

        var birth = new Date(value);
        if (isNaN(birth.getTime())) {
            return false;
        }

        var today = new Date();
        var age = today.getFullYear() - birth.getFullYear();
        var monthDiff = today.getMonth() - birth.getMonth();
        if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birth.getDate())) {
            age--;
        }

        return age >= years;
    });

    $.validator.unobtrusive.adapters.add("minimumage", ["years"], function (options) {
        options.rules["minimumage"] = parseInt(options.params.years, 10);
        options.messages["minimumage"] = options.message;
    });
})(jQuery);
