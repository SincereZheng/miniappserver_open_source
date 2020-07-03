/*
Bootstrap_Search_Suggest - v0.0.2
Description: 这是一个基于 bootstrap 按钮式下拉菜单组件的搜索建议插件，必须使用于按钮式下拉菜单组件上。
Author: lizhiwen#meizu.com
Github: https://github.com/lzwme/bootstrap-suggest-plugin
Update: 2015-11-18 12:24:39
*/
!
function(a) {
    a.fn.bsSuggest = function(a) {
        return "string" == typeof a && b[a] ? b[a].apply(this, Array.prototype.slice.call(arguments, 1)) : "object" != typeof a && a ? void 0 : b.init.apply(this, arguments)
    };
    var b = {
        init: function(b) {
            function c(b, c, d) {
                return c.is(":visible") ? void setTimeout(function() {
                    d.autoDropup && a(window).height() - b.offset().top < c.height() && b.offset().top > c.height() + a(window).scrollTop() ? c.parents(".input-group").addClass("dropup") : c.parents(".input-group.dropup").removeClass("dropup")
                },
                100) : ("left" === d.listAlign ? c.css({
                    left: b.siblings("div").width() - b.parent().width(),
                    right: "auto"
                }) : "right" === d.listAlign && c.css({
                    left: "auto",
                    right: "0"
                }), d.autoMinWidth === !1 ? c.css({
                    "min-width": b.parent().width()
                }) : c.css("width", "auto"), b)
            }
            function d(a, b) {
                var c, d, e;
                return - 1 === b.indexId && !b.idField || b.multiWord ? a: (c = a.css("background-color").replace(/ /g, "").split(",", 3).join(","), d = b.inputBgColor || "rgba(255,255,255,0.1)", e = b.inputWarnColor || "rgba(255,255,0,0.1)", !a.val() || a.attr("data-id") ? a.css("background", d) : ( - 1 === e.indexOf(c) && (a.trigger("onUnsetSelectValue"), a.css("background", e)), a))
            }
            function e(a, b) {
                var c, d, e = a.parent().find("tbody tr." + s.listHoverCSS);
                e.length > 0 && (c = (e.index() + 3) * e.height(), d = Number(b.css("max-height").replace("px", "")), b.scrollTop(c > d || b.scrollTop() > d ? c - d: 0))
            }
            function f(a, b) {
                a = a || $dropdownMenu,
                b = b || s,
                a.find("tr." + b.listHoverCSS).removeClass(b.listHoverCSS)
            }
            function g(b) {
                var c = a(b),
                d = c.parent(".input-group").find("ul.dropdown-menu"),
                e = c.data("bsSuggest");
                return 0 === d.length ? !1 : e ? !1 : (c.data("bsSuggest", {
                    target: b,
                    options: s
                }), !0)
            }
            function h(b, c, d, e) {
                var f, g, h, i, m, n, o, p = {
                    value: []
                };
                if (b = b || "", e = e || s, e.url) m = -1 !== e.url.indexOf("?") ? "&": "?",
                n = e.jsonp ? [e.url + b, m, e.jsonp, "=?"].join("") : e.url + b,
                a.ajax({
                    type: "GET",
                    url: urlFixFields(n),
                    dataType: "json",
                    timeout: 3e3
                }).done(function(a) {
                    d(c, a, e),
                    c.trigger("onDataRequestSuccess", a),
                    "firstByUrl" === s.getDataMethod && (s.data = a, s.url = null)
                }).fail(q);
                else {
                    if (f = e.data, g = j(f)) if (b) {
                        for (o = f.value.length, h = 0; o > h; h++) for (i in f.value[h]) if (a.trim(f.value[h][i]) && (l(i, e) || k(i, e)) && ( - 1 !== f.value[h][i].toString().indexOf(b) || -1 !== b.indexOf(f.value[h][i]))) {
                            p.value.push(f.value[h]);
                            break
                        }
                    } else p = f;
                    d(c, p, e)
                }
            }
            function i(a) {
                return validData = j(a)
            }
            function j(a) {
                var b = !0;
                for (var c in a) if ("value" === c) {
                    b = !1;
                    break
                }
                return b ? (q("返回数据格式错误!"), !1) : 0 === a.value.length ? !1 : a
            }
            function k(b, c) {
                return c = c || s,
                a.isArray(c.effectiveFields) && c.effectiveFields.length > 0 && -1 === a.inArray(b, c.effectiveFields) ? !1 : !0
            }
            function l(b, c) {
                return - 1 !== a.inArray(b, c.searchFields) ? !0 : !1
            }
            function m(a, b, d) {
                var e, f, g, h, j, l, m, o = a.parent().find("ul.dropdown-menu"),
                p = 0,
                q = ['<table class="table table-condensed">'];
                if (d = d || s, b = i(b), b === !1 || 0 === (e = b.value.length)) return o.empty().hide(),
                a;
                if (d.showHeader) {
                    h = "<thead><tr>";
                    for (g in b.value[0]) k(g) !== !1 && (h += 0 === p ? "<th>" + (d.effectiveFieldsAlias[g] || g) + "(" + e + ")</th>": "<th>" + (d.effectiveFieldsAlias[g] || g) + "</th>", p++);
                    h += "</tr></thead>",
                    q.push(h)
                }
                for (q.push("<tbody>"), f = 0; e > f; f++) {
                    p = 0,
                    j = "",
                    l = b.value[f][d.idField] || "",
                    m = b.value[f][d.keyField] || "";
                    for (g in b.value[f]) m || d.indexKey !== p || (m = b.value[f][g]),
                    l || d.indexId !== p || (l = b.value[f][g]),
                    p++,
                    k(g) !== !1 && (j += '<td data-name="' + g + '">' + b.value[f][g] + "</td>");
                    j = '<tr data-index="' + f + '" data-id="' + l + '" data-key="' + m + '">' + j + "</tr>",
                    q.push(j)
                }
                return q.push("</tbody></table>"),
                o.html(q.join("")).show(),
                n(a, o, d),
                o.css("max-height") && Number(o.css("max-height").replace("px", "")) < Number(o.find("table:eq(0)").css("height").replace("px", "")) && Number(o.css("min-width").replace("px", "")) < Number(o.css("width").replace("px", "")) ? o.css("padding-right", "20px").find("table:eq(0)").css("margin-bottom", "20px") : o.css("padding-right", 0).find("table:eq(0)").css("margin-bottom", 0),
                c(a, o, d),
                a
            }
            function n(b, c, e) {
                c = c || $dropdownMenu,
                e = e || s,
                c.find("tbody tr").each(function() {
                    a(this).off("mouseenter").on("mouseenter",
                    function() {
                        f(c, e),
                        a(this).addClass(e.listHoverCSS)
                    }).off("mousedown").on("mousedown",
                    function() {
                        p(b, o(a(this)), e),
                        d(b, e)
                    })
                })
            }
            function o(a) {
                var b = {};
                return b.id = a.attr("data-id"),
                b.key = a.attr("data-key"),
                b
            }
            function p(a, b, c) {
                var d, e = b || {},
                f = e.id || "",
                g = e.key || "";
                c && c.multiWord ? (d = a.val().split(c.separator || " "), d[d.length - 1] = g, a.val(d.join(c.separator || " ")).focus()) : a.attr("data-id", f).focus().val(g),
                a.trigger("onSetSelectValue", e)
            }
            function q(a, b) {
                console.trace(a),
                b && console.trace(b)
            }
            var r = this,
            s = a.extend({
                url: null,
                jsonp: null,
                data: {},
                getDataMethod: "firstByUrl",
                delayUntilKeyup: !1,
                indexId: 0,
                indexKey: 0,
                idField: "",
                keyField: "",
                effectiveFields: [],
                effectiveFieldsAlias: {},
                searchFields: [],
                showHeader: !1,
                showBtn: !0,
                allowNoKeyword: !0,
                multiWord: !1,
                separator: ",",
                processData: i,
                getData: h,
                autoMinWidth: !1,
                autoDropup: !1,
                listAlign: "left",
                inputBgColor: "",
                inputWarnColor: "rgba(255,0,0,.1)",
                listStyle: {
                    "padding-top": 0,
                    "max-height": "375px",
                    "max-width": "800px",
                    overflow: "auto",
                    width: "auto",
                    transition: "0.3s",
                    "-webkit-transition": "0.3s",
                    "-moz-transition": "0.3s",
                    "-o-transition": "0.3s"
                },
                listHoverStyle: "background: #07d; color:#fff",
                listHoverCSS: "jhover",
                keyLeft: 37,
                keyUp: 38,
                keyRight: 39,
                keyDown: 40,
                keyEnter: 13
            },
            b);
            if (a.isFunction(s.processData) && (i = s.processData), a.isFunction(s.getData) && (h = s.getData), !b.showHeader && s.effectiveFields && s.effectiveFields.length > 1 && (s.showHeader = !0), "firstByUrl" === s.getDataMethod && s.url && !s.delayUntilKeyup) {
                var t = -1 !== b.url.indexOf("?") ? "&": "?",
                u = b.jsonp ? [b.url, t, b.jsonp, "=?"].join("") : b.url;
                a.ajax({
                    type: "GET",
                    url: urlFixFields(u),
                    dataType: "json",
                    timeout: 5e3
                }).done(function(b) {
                    s.data = b,
                    s.url = null,
                    a(r).trigger("onDataRequestSuccess", b)
                }).fail(function(a, b) {
                    console.error(u + " : " + b)
                })
            }
            return a("head:eq(0)").append("<style>." + s.listHoverCSS + "{" + s.listHoverStyle + "}</style>"),
            r.each(function() {
                var i = a(this),
                j = i.parents(".input-group:eq(0)").find("ul.dropdown-menu");
                return g(this) === !1 ? void console.warn("不是一个标准的 bootstrap 下拉式菜单:", this) : (s.showBtn || i.css("border-radius", "4px").parents(".input-group:eq(0)").css("width", "100%").find(".input-group-btn>.btn").hide(), i.removeClass("disabled").attr("disabled", !1).attr("autocomplete", "off"), j.css(s.listStyle), s.inputBgColor || (s.inputBgColor = i.css("background-color")), i.on("keydown",
                function(b) {
                    var c, d = "";
                    "none" !== j.css("display") && (c = j.find("." + s.listHoverCSS), d = "", b.keyCode === s.keyDown ? (0 === c.length ? d = o(j.find("table tbody tr:first").mouseover()) : 0 === c.next().length ? (f(j, s), a(this).val(a(this).attr("alt")).attr("data-id", "")) : (f(j, s), 0 !== c.next().length && (d = o(c.next().mouseover()))), e(i, j)) : b.keyCode === s.keyUp ? (0 === c.length ? d = o(j.find("table tbody tr:last").mouseover()) : 0 === c.prev().length ? (f(j, s), a(this).val(a(this).attr("alt")).attr("data-id", "")) : (f(j, s), 0 !== c.prev().length && (d = o(c.prev().mouseover()))), e(i, j)) : b.keyCode === s.keyEnter ? j.hide().empty() : a(this).attr("data-id", ""), d && "" !== d.key && p(a(this), d, s))
                }).on("keyup",
                function(c) {
                    var e, f;
                    return c.keyCode === s.keyDown || c.keyCode === s.keyUp || c.keyCode === s.keyEnter ? (a(this).val(a(this).val()), void d(i, s)) : (a(this).attr("data-id", ""), d(i, s), e = a(this).val(), void(("" === a.trim(e) || e !== a(this).attr("alt")) && (a(this).attr("alt", a(this).val()), b.multiWord && (f = e.split(s.separator || " "), e = f[f.length - 1]), (0 !== e.length || s.allowNoKeyword) && h(a.trim(e), i, m, s))))
                }).on("focus",
                function() {
                    c(i, j, s)
                }).on("blur",
                function() {
                    j.css("display", "")
                }).on("click",
                function() {
                    var b, c = a(this).val();
                    return "" !== a.trim(c) && c === a(this).attr("alt") && j.find("table tr").length ? j.show() : void("none" === j.css("display") && (s.multiWord && (b = c.split(s.separator || " "), c = b[b.length - 1]), (0 !== c.length || s.allowNoKeyword) && h(a.trim(c), i, m, s)))
                }), i.parent().find("button:eq(0)").attr("data-toggle", "").on("click",
                function() {
                    var a;
                    "none" === j.css("display") ? (a = "block", s.url ? i.click().focus() : (m(i, s.data, s), c(i, j, s))) : a = "none",
                    j.css("display", a)
                }), void j.on("mouseenter",
                function() {
                    a(this).show()
                }).on("mouseleave",
                function() {
                    i.focus()
                }))
            })
        },
        show: function() {
            var a = this.data("bsSuggest");
            return a && a.options && this.parent().find("ul.dropdown-menu").show(),
            this
        },
        hide: function() {
            var a = this.data("bsSuggest");
            return a && a.options && this.parent().find("ul.dropdown-menu").css("display", ""),
            this
        },
        disable: function() {
            return a(this).data("bsSuggest") ? void a(this).attr("disabled", !0).parent().find(".input-group-btn>.btn").addClass("disabled") : !1
        },
        enable: function() {
            return a(this).data("bsSuggest") ? void a(this).attr("disabled", !1).parent().find(".input-group-btn>.btn").removeClass("disabled") : !1
        },
        destroy: function() {
            a(this).off().removeData("bsSuggest").parent().find(".input-group-btn>.btn").off()
        },
        version: function() {
            return "0.0.1"
        }
    }
} (window.jQuery);