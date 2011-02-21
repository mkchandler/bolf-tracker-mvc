var question = function () {
    var a = function () {
        return $('div#question div.post-menu a:contains("link")')
    };
    return {
        showShareTip: function () {
            var c = a();
            if (c.hasClass("share-link")) {
                return
            }
            c.addClass("share-link");
            var b = '<div class="share-tip">share a link to this hot question to earn the <a href="/badges">publicist badge</a><input type="text" value="http://' + document.location.host + c.attr("href") + '" style="display:block; width:292px;"><a onclick="question.hideShareTip()" style="float:right">close</a></div>';
            c.parent().append(b)
        },
        hideShareTip: function () {
            var b = a();
            b.removeClass("share-link");
            b.parent().find(".share-tip").fadeOutAndRemove()
        }
    }
} ();
var vote = function () {
    var voteTypeIds = {
        informModerator: -1,
        undoMod: 0,
        acceptedByOwner: 1,
        upMod: 2,
        downMod: 3,
        offensive: 4,
        favorite: 5,
        close: 6,
        reopen: 7,
        bountyClose: 9,
        deletion: 10,
        undeletion: 11,
        spam: 12
    };
    var fetchVoteTitle = "View upvote and downvote totals";
    var bindAnonymousDisclaimers = function () {
        var anchor = '<a href="/users/login?returnurl=' + escape(document.location) + '">login or register</a>';
        unbindVoteClicks().click(function () {
            showNotification($(this), "Please " + anchor + " to vote for this post.")
        });
        getFlagLinks().unbind("click").click(function () {
            showNotification($(this), "Please " + anchor + " to flag this post.")
        })
    };
    var promptToLogin = false;
    var showPromptToLogin = function (jClicked) {
        if (promptToLogin) {
            var verb = jClicked.is('[id^="flag-post-"]') ? "flag" : jClicked.is(".star-off") ? "favorite" : "vote for";
            showNotification(jClicked, 'Please <a href="/users/login?returnurl=' + escape(document.location) + '">login or register</a> to ' + verb + " this post.")
        }
        return promptToLogin
    };
    var bindVoteClicks = function (jDivVote) {
        if (!jDivVote) {
            jDivVote = "div.vote"
        }
        $(jDivVote).find(".vote-up-off").unbind("click").click(function () {
            vote.up($(this))
        });
        $(jDivVote).find(".vote-down-off").unbind("click").click(function () {
            vote.down($(this))
        })
    };
    var unbindVoteClicks = function (jClicked) {
        var jDiv = jClicked ? jClicked.closest("div.vote") : $("div.vote");
        return jDiv.find(".vote-up-off, .vote-down-off").unbind("click")
    };
    var fetchVotesCast = function (questionId) {
        $.ajax({
            type: "GET",
            url: "/posts/" + questionId + "/votes",
            dataType: "json",
            success: vote.highlightExistingVotes
        })
    };
    var getAcceptedAnswerLinks = function () {
        return $("div.vote a[id^='vote-accepted-']")
    };
    var getLockPostLinks = function () {
        return $("div.post-menu a[id^='lock-post-']")
    };
    var getFlagLinks = function () {
        return $("div.post-menu a[id^='flag-post-']")
    };
    var getProtectLinks = function () {
        return $("div.post-menu a[id^='protect-post-']")
    };
    var getUnprotectLinks = function () {
        return $("div.post-menu a[id^='unprotect-post-']")
    };
    var isUpSelected = function (jUp) {
        return jUp.hasClass("vote-up-on")
    };
    var isDownSelected = function (jDown) {
        return jDown.hasClass("vote-down-on")
    };
    var isFavoriteSelected = function (jFavorite) {
        return jFavorite.hasClass("star-on")
    };
    var getPostId = function (jClicked) {
        return jClicked.closest("div.vote").find("input").val()
    };
    var reset = function (jUp, jDown) {
        jUp.removeClass("vote-up-on");
        jDown.removeClass("vote-down-on")
    };
    var updateModScore = function (jClicked, incrementAmount) {
        var jScore = jClicked.siblings("span.vote-count-post");
        var currentScore;
        if (jScore.find(".vote-count-separator").length > 0) {
            var up = parseInt(jScore.children(":first").text(), 10);
            var down = Math.abs(parseInt(jScore.children(":last").text(), 10));
            currentScore = up - down;
            jScore.css("cursor", "pointer").unbind("click").attr("title", fetchVoteTitle).click(function () {
                vote.fetchVoteCounts($(this))
            })
        } else {
            currentScore = parseInt(jScore.text(), 10)
        }
        jScore.text(currentScore + incrementAmount)
    };
    var submitModVote = function (jClicked, voteTypeId) {
        unbindVoteClicks(jClicked);
        var postId = getPostId(jClicked);
        submit(jClicked, postId, voteTypeId, modVoteResult)
    };
    var submit = function (jClicked, postId, voteTypeId, callback, optionalFormData, completeCallback) {
        var formData = {
            fkey: fkey
        };
        if (optionalFormData) {
            for (var name in optionalFormData) {
                formData[name] = optionalFormData[name]
            }
        }
        $.ajax({
            type: "POST",
            url: "/posts/" + postId + "/vote/" + voteTypeId,
            data: formData,
            dataType: "json",
            success: function (data) {
                callback(jClicked, postId, data)
            },
            error: function () {
                showNotification(jClicked, "An error has occurred - please retry your request.")
            },
            complete: completeCallback
        })
    };
    var modVoteResult = function (jClicked, postId, data) {
        if (data.Success) {
            if (data.Message) {
                showFadingNotification(jClicked, data.Message)
            }
            if (data.ShowShareTip) {
                question.showShareTip()
            }
        } else {
            if (window.console && window.console.firebug && (!data.Message || data.Message.length < 5)) {
                showNotification(jClicked, "FireBug seems to be enabled, which can sometimes interfere with voting;<br>please refresh the page to see if your vote was processed.<br><br>If this persists, consider disabling FireBug for this site.")
            } else {
                showNotification(jClicked, data.Message);
                reset(jClicked, jClicked);
                jClicked.parent().find("span.vote-count-post").text(data.NewScore);
                if (data.LastVoteTypeId) {
                    selectPreviousVote(jClicked, data.LastVoteTypeId)
                }
            }
        }
        bindVoteClicks(jClicked.parent())
    };
    var selectPreviousVote = function (jClicked, voteTypeId) {
        var span, spanSelectedClass;
        if (voteTypeId == voteTypeIds.upMod) {
            span = ".vote-up-off";
            spanSelectedClass = "vote-up-on"
        } else {
            if (voteTypeId == voteTypeIds.downMod) {
                span = ".vote-down-off";
                spanSelectedClass = "vote-down-on"
            }
        }
        if (span) {
            jClicked.closest("div.vote").find(span).addClass(spanSelectedClass)
        }
    };
    var showNotification = function (jClicked, msg) {
        master.showErrorPopup(jClicked.parent(), msg)
    };
    var showFadingNotification = function (jClicked, msg) {
        master.showErrorPopup(jClicked.parent(), msg, true)
    };
    var confirmBountyAward = function (postId) {
        if (typeof hasOpenBounty != "undefined" && hasOpenBounty) {
            return confirm("Are you sure you want to award your bounty to this answer? THIS CANNOT BE UNDONE!")
        }
        return true
    };
    return {
        init: function (questionId) {
            promptToLogin = !(typeof isRegistered != "undefined" && isRegistered);
            if (promptToLogin) {
                bindAnonymousDisclaimers()
            } else {
                if (votesCast == null) {
                    fetchVotesCast(questionId)
                } else {
                    vote.highlightExistingVotes(votesCast)
                }
                bindVoteClicks();
                getFlagLinks().unbind("click").click(function () {
                    vote.flag($(this))
                });
                getProtectLinks().unbind("click").click(function () {
                    var id = this.id.substring("protect-post-".length);
                    if (confirm("Are you sure you want to protect this question?")) {
                        $.ajax({
                            type: "POST",
                            url: "/question/protect",
                            data: {
                                id: id,
                                fkey: fkey
                            },
                            success: function (data) {
                                location.reload(true)
                            }
                        })
                    }
                    return false
                });
                getUnprotectLinks().unbind("click").click(function () {
                    var id = this.id.substring("unprotect-post-".length);
                    if (confirm("Are you sure you want to unprotect this question?")) {
                        $.ajax({
                            type: "POST",
                            url: "/question/unprotect",
                            data: {
                                id: id,
                                fkey: fkey
                            },
                            success: function (data) {
                                location.reload(true)
                            }
                        })
                    }
                    return false
                })
            }
            vote.favorite_init();
            vote.bountyClose_init();
            getAcceptedAnswerLinks().unbind("click").click(function () {
                vote.acceptedAnswer($(this))
            });
            var jCloseLink = $("div.post-menu a[id^='close-question-']");
            jCloseLink.unbind("click").click(function () {
                vote.close(jCloseLink)
            });
            vote.init_delete()
        },
        init_delete: function (optionalCallback) {
            $("div.post-menu *[id^='delete-post-']").unbind("click").click(function () {
                vote.deletion($(this), optionalCallback)
            })
        },
        up: function (jClicked) {
            var jUp = jClicked.parent().find(".vote-up-off");
            var jDown = jClicked.parent().find(".vote-down-off");
            var isSelected = isUpSelected(jUp);
            var isReversal = isDownSelected(jDown);
            var incrementAmount = isSelected ? -1 : (isReversal ? 2 : 1);
            updateModScore(jClicked, incrementAmount);
            reset(jUp, jDown);
            if (!isSelected) {
                jUp.addClass("vote-up-on")
            }
            submitModVote(jClicked, isSelected ? voteTypeIds.undoMod : voteTypeIds.upMod)
        },
        down: function (jClicked) {
            var jUp = jClicked.parent().find(".vote-up-off");
            var jDown = jClicked.parent().find(".vote-down-off");
            var isSelected = isDownSelected(jDown);
            var isReversal = isUpSelected(jUp);
            var incrementAmount = isSelected ? 1 : (isReversal ? -2 : -1);
            updateModScore(jClicked, incrementAmount);
            reset(jUp, jDown);
            if (!isSelected) {
                jDown.addClass("vote-down-on")
            }
            submitModVote(jClicked, isSelected ? voteTypeIds.undoMod : voteTypeIds.downMod)
        },
        favorite_init: function () {
            $(".star-off:not(.disabled)").live("click", function (evt) {
                vote.favorite($(this));
                evt.preventDefault()
            })
        },
        favorite: function (jClicked) {
            if (showPromptToLogin(jClicked)) {
                return
            }
            jClicked.addClass("disabled");
            var jFavoriteCount = jClicked.parent().find("div.favoritecount b");
            var count = parseInt("0" + jFavoriteCount.text().replace(/^\s+|\s+$/g, ""), 10);
            if (isFavoriteSelected(jClicked)) {
                jClicked.removeClass("star-on");
                jFavoriteCount.removeClass("favoritecount-selected").text((count-- <= 0) ? "" : count)
            } else {
                jClicked.addClass("star-on");
                jFavoriteCount.addClass("favoritecount-selected").text(++count)
            }
            var postId = getPostId(jClicked) || jClicked.siblings("input[type=hidden]").val();
            submit(jClicked, postId, voteTypeIds.favorite, function (data) {
                jClicked.removeClass("disabled")
            })
        },
        acceptedAnswer: function (jClicked) {
            var postId = jClicked.attr("id").substring("vote-accepted-".length);
            getAcceptedAnswerLinks().unbind("click");
            submit(jClicked, postId, voteTypeIds.acceptedByOwner, function (jClicked, postId, data) {
                if (data.Success) {
                    $(".vote-accepted-off").removeClass("vote-accepted-on");
                    var typeId = parseInt(data.Message, 10);
                    if (typeId == voteTypeIds.acceptedByOwner) {
                        jClicked.addClass("vote-accepted-on")
                    } else {
                        jClicked.removeClass("vote-accepted-on")
                    }
                } else {
                    showNotification(jClicked, data.Message)
                }
                getAcceptedAnswerLinks().click(function () {
                    vote.acceptedAnswer($(this))
                })
            })
        },
        bountyClose_init: function () {
            $(".bounty-vote").hover(function () {
                $(this).removeClass("bounty-vote-off")
            }, function () {
                $(this).addClass("bounty-vote-off")
            });
            $(".bounty-vote:not(.disabled)").live("click", function () {
                vote.bountyClose($(this))
            })
        },
        bountyClose: function (jClicked) {
            var postId = getPostId(jClicked);
            if (!confirmBountyAward(postId)) {
                return
            }
            $(".bounty-vote").addClass("disabled");
            submit(jClicked, postId, voteTypeIds.bountyClose, vote.bountyClose_callback)
        },
        highlightExistingVotes: function (jsonArray) {
            $.each(jsonArray, function () {
                var jDiv = $("div.vote:has(input[value=" + this.PostId + "])");
                switch (this.VoteTypeId) {
                    case voteTypeIds.upMod:
                        jDiv.find(".vote-up-off").addClass("vote-up-on");
                        break;
                    case voteTypeIds.downMod:
                        jDiv.find(".vote-down-off").addClass("vote-down-on");
                        break;
                    case voteTypeIds.favorite:
                        jDiv.find(".star-off").addClass("star-on");
                        jDiv.find("div.favoritecount b").addClass("favoritecount-selected");
                        break;
                    default:
                        alert("site.vote.js > highlightExistingVotes has no case for " + this.VoteTypeId);
                        break
                }
            });
            votesCast = null
        },
        bountyClose_callback: function (jClicked, postId, data) {
            var jButtons = $(".bounty-vote");
            if (data.Success) {
                $("#bounty-notification").remove();
                hasOpenBounty = false;
                var jRep = jClicked.closest("td.votecell").parent().find("span.reputation-score:last");
                if (data.Message) {
                    var jData = $(data.Message);
                    jRep.text(jData.text()).attr("title", jData.attr("title"))
                }
                var jContainers = jClicked.closest("div.vote").find(".bounty-award-container");
                if (jContainers.length > 1) {
                    var jAward = jContainers.filter(":first").find(".bounty-award");
                    var previousAward = parseInt(jAward.text(), 10);
                    var currentAward = parseInt(jClicked.text(), 10);
                    jAward.text("+" + (previousAward + currentAward));
                    jButtons.remove()
                } else {
                    jClicked.unbind("mouseenter mouseleave").removeClass("bounty-vote bounty-vote-off");
                    jButtons.not(jClicked).remove()
                }
            } else {
                showNotification(jClicked, data.Message);
                jButtons.removeClass("disabled")
            }
        },
        flag: function (clicked) {
            var postId = clicked.attr("id").substring("flag-post-".length);
            clicked.loadPopup({
                url: "/posts/popup/flag/" + postId,
                loaded: vote.flag_loaded,
                hideDescriptions: true,
                actionSelected: vote.flag_actionSelected,
                subformShow: vote.flag_subformShow
            })
        },
        flag_loaded: function (popup) {
            vote.flag_cachedClosePopup = null;
            vote.flag_initAttentionSubform(popup);
            popup.find("form").submit(function () {
                vote.flag_submit(popup);
                return false
            });
            $("#flag-load-close").unbind("click").click(function () {
                vote.flag_showCloseDialog(popup, $(this))
            })
        },
        flag_actionSelected: function (li) {
            var voteType = li.find("input:radio").attr("id").substr("flag-".length);
            var isSpam = voteType == voteTypeIds.offensive || voteType == voteTypeIds.spam;
            $(".flag-remaining-spam").toggle(isSpam);
            $(".flag-remaining-inform").toggle(!isSpam)
        },
        flag_initAttentionSubform: function (popup) {
            var submit = popup.find(".popup-submit");
            var txt = popup.find('textarea[name="flag-reason"]');
            txt.focus(function () {
                txt.hideHelpOverlay();
                var radio = txt.closest("li").find("input:radio");
                if (!radio.is(":checked")) {
                    radio.attr("checked", "checked")
                }
            }).charCounter({
                min: 10,
                max: 500,
                setIsValid: function (isValid) {
                    submit.enable(isValid)
                }
            });
            var sub = txt.closest(".action-subform");
            sub.find("input.flag-prefilled").click(function () {
                submit.enable()
            });
            sub.find('input[value="other"]').click(function () {
                txt.focus()
            });
            sub.find("label, input:radio").css("cursor", "pointer")
        },
        flag_subformShow: function (subform) {
            if (!subform.is(".mod-attention-subform")) {
                return
            }
            var reason = subform.find('textarea[name="flag-reason"]');
            reason.helpOverlay();
            if (subform.find("input[value=other]:checked").length) {
                reason.focus()
            } else {
                if (!subform.find("input:radio:checked").length) {
                    subform.closest(".popup").find(".popup-submit").disable()
                }
            }
        },
        flag_showCloseDialog: function (popup, radio) {
            radio.closest("li").siblings("li").trigger("hide-action");
            if (vote.flag_cachedClosePopup) {
                popup.fadeOut("fast");
                vote.flag_cachedClosePopup.fadeIn("fast")
            } else {
                radio.siblings(".action-name").addSpinner({
                    margin: "0 5px"
                });
                var postId = popup.attr("id").substr("flag-popup-".length);
                var flagLink = $("#flag-post-" + postId);
                var isForFlagging = $("#flag-isClosePopupForFlagging").val() == "true";
                vote.close_loadPopup(flagLink, postId, isForFlagging, popup)
            }
            popup.find(".popup-submit").disable();
            radio.removeAttr("checked")
        },
        flag_cachedClosePopup: null,
        flag_submit: function (popup) {
            popup.find(".popup-submit").disable().siblings(".spinner-container").addSpinner();
            var postId = popup.attr("id").substr("flag-popup-".length);
            var flagLink = $("#flag-post-" + postId);
            var vType = popup.find('input[name="flag-post"]:checked').val();
            if (vType == voteTypeIds.informModerator) {
                vote.flag_submitInformModerator(popup, postId, flagLink)
            } else {
                submit(flagLink, postId, vType, vote.flag_submitCallback, null, function () {
                    popup.fadeOutAndRemove()
                })
            }
        },
        flag_submitInformModerator: function (popup, postId, flagLink) {
            var subform = popup.find(".mod-attention-subform");
            var radio = subform.find("input:radio:checked");
            var msg = radio.val() == "other" ? subform.find('textarea[name="flag-reason"]').val() : radio.val();
            $.ajax({
                type: "POST",
                url: "/messages/inform-moderator-about-post/" + postId,
                dataType: "json",
                data: {
                    fkey: fkey,
                    msg: msg
                },
                success: function (json) {
                    flagLink.parent().showErrorPopup(json.Message)
                },
                error: function (res, textStatus, errorThrown) {
                    flagLink.parent().showErrorPopup(res.responseText && res.responseText.length < 100 ? res.responseText : "An error occurred during submission")
                },
                complete: function () {
                    popup.fadeOutAndRemove()
                }
            })
        },
        flag_submitCallback: function (flagLink, postId, data) {
            if (data.Refresh) {
                location.reload(true)
            } else {
                flagLink.parent().showErrorPopup(data.Message)
            }
        },
        close: function (closeLink) {
            var isClosed = closeLink.text().indexOf("open") > -1;
            var postId = closeLink.attr("id").substring("close-question-".length);
            if (isClosed) {
                if (confirm("Nominate this question for reopening?")) {
                    submit(closeLink, postId, voteTypeIds.reopen, vote.close_submitComplete)
                }
            } else {
                vote.close_loadPopup(closeLink, postId)
            }
        },
        close_loadPopup: function (closeLink, postId, isForFlagging, flagPopup) {
            var postMenu = closeLink.parent();
            if (!isForFlagging) {
                closeLink.addSpinnerAfter({
                    padding: "0 3px"
                })
            }
            $.ajax({
                type: "GET",
                url: "/posts/popup/close/" + postId + (isForFlagging ? "?isForFlagging=1" : ""),
                dataType: "html",
                success: function (html) {
                    var popup = $(html);
                    if (flagPopup) {
                        flagPopup.fadeOut("fast");
                        vote.flag_cachedClosePopup = popup
                    }
                    popup.appendTo(postMenu);
                    vote.close_initPopup(popup, closeLink, flagPopup);
                    popup.center().fadeIn("fast")
                },
                error: function () {
                    postMenu.showErrorPopup("Unable to load popup - please try again")
                },
                complete: master.removeSpinner
            })
        },
        close_initPopup: function (popup, closeLink, flagPopup) {
            popup.find(".popup-close").click(function () {
                popup.fadeOutAndRemove();
                if (flagPopup) {
                    flagPopup.remove()
                }
            });
            var cancelLink = popup.find(".popup-actions-cancel");
            if (flagPopup) {
                cancelLink.text("back").click(function () {
                    popup.fadeOut("fast");
                    flagPopup.fadeIn("fast")
                })
            } else {
                cancelLink.click(function () {
                    popup.fadeOutAndRemove()
                })
            }
            var submitButton = popup.find(".popup-submit");
            popup.find('input[type=radio]:not(input[name="existing-close"])').click(function () {
                var rb = $(this);
                var li = rb.closest("li");
                if (li.hasClass("action-selected")) {
                    return
                }
                if (!vote.close_showSubPane(rb)) {
                    submitButton.enable();
                    popup.find(".popup-active-pane li.action-selected").removeClass("action-selected");
                    li.addClass("action-selected")
                }
            });
            var txt = $("#duplicate-question");
            var hidden = $("#duplicate-question-id");
            txt.helpOverlay().typeWatch({
                highlight: false,
                wait: 750,
                captureLength: -1,
                callback: function () {
                    vote.close_duplicateTypeWatch(submitButton, txt, questionId, hidden)
                }
            });
            $(".existing-linked-questions input[type=radio]").click(function () {
                txt.hideHelpOverlay().val($(this).parent().attr("title")).keydown()
            });
            $("#close-question-form").submit(function () {
                vote.close_submit(popup, closeLink, $(this), hidden, submitButton);
                return false
            })
        },
        close_submit: function (popup, closeLink, form, hidden, submitButton) {
            if (submitButton.attr("disabled")) {
                return
            }
            $("#remaining-votes").addSpinner({
                position: "absolute",
                margin: "3px 0 0 5px"
            });
            submitButton.disable();
            var isForFlagging = (popup.find("input[name=isForFlagging]").length > 0);
            $.ajax({
                type: "POST",
                url: form.attr("action"),
                dataType: "json",
                data: {
                    fkey: fkey,
                    isForFlagging: isForFlagging,
                    "close-reason-id": $("#pane-main input[type=radio]:checked").val(),
                    "migration-site-id": $('input[type=radio][id^="close-offtopic-"]:checked').val(),
                    "duplicate-question-id": hidden.val()
                },
                success: function (data) {
                    vote.close_submitComplete(closeLink, questionId, data, isForFlagging)
                },
                error: function (res, textStatus, errorThrown) {
                    closeLink.parent().showErrorPopup("A problem occurred when trying to " + closeLink.text().indexOf("open") > -1 ? "reopen" : "close")
                },
                complete: function () {
                    master.removeSpinner();
                    popup.fadeOutAndRemove()
                }
            })
        },
        close_submitComplete: function (closeLink, postId, data, isForFlagging) {
            var verb = closeLink.text().indexOf("open") > -1 ? "reopen" : "close";
            if (data.Success) {
                if (data.Message) {
                    var postMenu = closeLink.parent();
                    if (isForFlagging) {
                        postMenu.showErrorPopup(data.Message)
                    } else {
                        closeLink.text(closeLink.text().replace(/\w?\(\d\)/, "") + " " + data.Message);
                        postMenu.showErrorPopup("This question still needs " + data.NewScore + " vote(s) from other users to " + verb)
                    }
                } else {
                    if (data.RedirectTo) {
                        location.href = data.RedirectTo
                    } else {
                        location.reload(true)
                    }
                }
            } else {
                closeLink.parent().showErrorPopup(data.Message || "A problem occurred when trying to " + verb)
            }
        },
        close_showSubPane: function (rb) {
            var id = rb.attr("id").substr("close-".length);
            var pane = $("#pane" + id);
            if (pane.length == 0) {
                return false
            }
            $("#pane-main").removeClass("popup-active-pane").hide();
            pane.addClass("popup-active-pane").show();
            rb.closest("div.popup").find(".popup-actions-cancel").html("back").unbind("click").click(vote.close_showMainPane);
            eval("pane" + id + "()");
            return true
        },
        close_showMainPane: function () {
            $(".popup-subpane").removeClass("popup-active-pane").hide();
            $("#pane-main").addClass("popup-active-pane").show().find("input[type=radio]:checked").removeAttr("checked").end().find("li.action-selected").removeClass("action-selected");
            $(".popup").find(".popup-submit").disable().end().find(".popup-actions-cancel").html("cancel").unbind("click").click(function () {
                $(".popup").fadeOutAndRemove()
            })
        },
        close_duplicateTypeWatch: function (jSubmit, jText, postId, jHidden) {
            var jPreview = jText.parent().find(".selected-master-preview");
            jSubmit.disable();
            jPreview.html("");
            if ($.trim(jText.val()).length == 0) {
                return
            }
            appendLoader(jPreview);
            $.ajax({
                type: "GET",
                url: "/posts/validate-duplicate-question/" + postId,
                data: {
                    val: jText.val()
                },
                dataType: "json",
                success: function (json) {
                    if (json.success) {
                        jHidden.val(json.id);
                        vote.close_showMasterPreview(jPreview, json);
                        jSubmit.enable()
                    } else {
                        if (json.title) {
                            jPreview.html(json.title)
                        } else {
                            jPreview.html("")
                        }
                    }
                },
                error: function (res, textStatus, errorThrown) {
                    removeLoader();
                    showAjaxError(jText.parent(), (res.responseText && res.responseText.length < 100 ? res.responseText : "An error occurred during duplicate validation"))
                }
            })
        },
        close_showMasterPreview: function (jPreview, json) {
            var html = '<div class="post-text" style="max-height: 400px; overflow: auto;"><h3><a href="' + json.url + '" target="_blank">' + json.title + "</a></h3><p>" + json.body + "</p></div><p>" + json.tags + "</p>";
            jPreview.html(html)
        },
        deletion: function (jClicked, optionalCallback) {
            var postId = jClicked.attr("id").substring("delete-post-".length);
            var isDeleted = jClicked.text().indexOf("undelete") > -1;
            if (confirm("Vote to " + (isDeleted ? "un" : "") + "delete this post?")) {
                submit(jClicked, postId, (isDeleted ? voteTypeIds.undeletion : voteTypeIds.deletion), optionalCallback || vote.deletionCallback)
            }
        },
        deletionCallback: function (jClicked, postId, data) {
            var wasDeleted = jClicked.text().indexOf("undelete") > -1;
            if (data && data.Success) {
                jClicked.text(data.Message);
                if (data.NewScore < 0) {
                    var isQuestion = $("#question:has(a[id='delete-post-" + postId + "'])").length > 0;
                    var selector = isQuestion ? "#question, div.answer" : "#answer-" + postId;
                    vote.setDeleteStyles($(selector), !wasDeleted)
                } else {
                    showNotification(jClicked, "This post still needs " + data.NewScore + " vote(s) from other users to " + (wasDeleted ? "un" : "") + "delete")
                }
            } else {
                var msg = (data && data.Message) ? data.Message : "A problem occurred during " + (wasDeleted ? "un" : "") + "deletion";
                showAjaxError(jClicked.parent(), msg)
            }
        },
        setDeleteStyles: function (jDiv, isDeleted) {
            if (isDeleted) {
                $("div.question-status:has(span:contains('delete'))").show();
                jDiv.addClass("deleted-answer").find("a[id^='delete-post-']").addClass("deleted-post").end().find("div[id^='comments-']").addClass("comments-container-deleted").end().find("a[id^='comments-link-']").addClass("comments-link-deleted")
            } else {
                document.location.reload(true)
            }
        },
        bounty_init: function (link) {
            var link = $("#bounty-link");
            link.click(function () {
                $("#bounty").toggle();
                link.text(link.text().indexOf("start") > -1 ? "hide bounty" : "start a bounty")
            });
            var button = $("#bounty-start");
            button.click(function () {
                var amount = $("#bounty-amount").val();
                if (!confirm("Are you sure you want to start a bounty of " + amount + " on this question?")) {
                    return
                }
                button.disable();
                vote.bounty_start(amount)
            })
        },
        bounty_start: function (amount) {
            var questionId = $("#question div.vote input:first").val();
            var div = $("#bounty");
            var button = $("#bounty-start");
            button.addSpinnerAfter({
                "padding-left": "3px"
            });
            $.ajax({
                type: "POST",
                url: "/posts/" + questionId + "/bounty-start",
                dataType: "json",
                data: {
                    fkey: fkey,
                    amount: amount
                },
                success: function (data) {
                    if (data.Success) {
                        location.reload(true)
                    } else {
                        div.showErrorPopup(data.Message);
                        button.enable()
                    }
                },
                error: function (res, textStatus, errorThrown) {
                    div.showErrorPopup(res.responseText && res.responseText.length < 100 ? res.responseText : "An error occurred when starting bounty")
                },
                complete: master.removeSpinner
            })
        },
        bindFetchVoteCounts: function () {
            $(".vote-count-post").attr("title", fetchVoteTitle).css("cursor", "pointer").unbind("click").click(function () {
                vote.fetchVoteCounts($(this))
            })
        },
        fetchVoteCounts: function (jScore) {
            var postId = jScore.closest("div.vote").children('input[type="hidden"]').val();
            appendLoader(jScore);
            $.ajax({
                type: "GET",
                url: "/posts/" + postId + "/vote-counts",
                dataType: "json",
                success: function (json) {
                    $(".error-notification").fadeOut("fast", function () {
                        $(this).remove()
                    });
                    var html = '<div style="color:green">' + json.up + '</div><div class="vote-count-separator"></div><div style="color:maroon">' + json.down + "</div>";
                    jScore.html(html).unbind("click").css("cursor", "default").attr("title", (json.up + " up / " + json.down + " down"))
                },
                error: function (res, textStatus, errorThrown) {
                    removeLoader();
                    showAjaxError(jScore.parent(), (res.responseText && res.responseText.length < 100 ? res.responseText : "An error occurred during vote count fetch"))
                }
            })
        }
    }
} ();
var comments = function () {
    var e;
    var a = 600;
    var d = function (q, r) {
        return $("div#comments-" + q + (r || ""))
    };
    var c = function () {
        $("a[id^='comments-link-']").unbind("click").click(function () {
            var u = $(this).attr("id").substr("comments-link-".length);
            var t = d(u);
            var q = t.find('form[id^="add-comment-"]');
            if (q.children().length == 0) {
                n(u);
                t.removeClass("dno");
                if ($(this).text().indexOf("more comment") > -1) {
                    comments.fetch(u, t)
                } else {
                    var s = 200 + ($('form[id^="add-comment-"] > table').length * 2);
                    var r = t.find("tfoot form textarea");
                    r.attr("tabindex", s++);
                    t.find("tfoot form input").attr("tabindex", s);
                    if (!r.closest("form").hasClass(".comment-form-expanded")) {
                        r.focus()
                    }
                }
            } else {
                q.show().find("textarea").focus()
            }
            $(this).hide().text("add comment")
        })
    };
    var j = function (q) {
        $(".comment-up").click(function () {
            m($(this), 2, "comment-up", "comment-up-on", function (s, r) {
                s.closest("tr").siblings("tr").remove();
                s.parent().siblings().children().remove();
                s.parent().siblings().append(b(r.NewScore))
            })
        }).hover(function () {
            $(this).addClass("comment-up-on")
        }, function () {
            $(this).removeClass("comment-up-on")
        });
        $(".comment-flag").click(function () {
            if (confirm("Really flag this comment as noise, offensive or spam?")) {
                m($(this), 4, "comment-flag", "flag-on", function (s, r) {
                    if (r.NewScore == -1) {
                        s.parents("tr.comment").remove()
                    } else {
                        s.parents("tr.comment").find("img.comment-up").remove();
                        s.remove()
                    }
                })
            }
        }).hover(function () {
            $(this).addClass("flag-on")
        }, function () {
            $(this).removeClass("flag-on")
        });
        $("div.comments a.comment-edit, td.comment-summary a.comment-edit").click(function () {
            h($(this))
        });
        $(".comment-delete").click(function () {
            if (confirm("Really delete this comment?")) {
                m($(this), 10, "comment-delete", "delete-tag-hover", function (s, t) {
                    var r = s.parents("tr.comment");
                    if (q) {
                        q(r)
                    }
                    r.remove()
                })
            }
        }).hover(function () {
            $(this).addClass("delete-tag-hover")
        }, function () {
            $(this).removeClass("delete-tag-hover")
        });
        if (e) {
            $("tr.comment").find(".comment-delete, a.comment-edit-hide").css("visibility", "visible")
        } else {
            $("tr.comment").hover(function () {
                $(this).addClass("comment-hover").find(".comment-up, .comment-flag, .comment-delete, a.comment-edit-hide").css("visibility", "visible")
            }, function () {
                $(this).removeClass("comment-hover").find(".comment-up, .comment-flag, .comment-delete, a.comment-edit-hide").css("visibility", "hidden")
            })
        }
    };
    var o = function () {
        $("tr.comment").unbind("mouseenter mouseleave");
        $(".comment-up, .comment-flag, .comment-delete, div.comments a.comment-edit").unbind("click mouseenter mouseleave")
    };
    var g = function (s, q) {
        var r = d(s, " > table > tbody");
        if (r.children().length > 0) {
            r.children().remove()
        }
        r.append(q);
        o();
        j();
        removeLoader();
        if (typeof MathJax != "undefined") {
            MathJax.Hub.Queue(["Typeset", MathJax.Hub])
        }
    };
    var b = function (q) {
        var s = "";
        if (q && q > 0) {
            var r = q < 5 ? "" : q <= 15 ? "warm" : q <= 30 ? "hot" : "supernova";
            s += '<span title="number of \'great comment\' votes received" class="' + r + '">' + q + "</span>"
        }
        return s
    };
    var l = function (x, r, u, v) {
        var t = '<table><tr><td><textarea name="comment" cols="68" rows="3"></textarea></td><td><input type="submit" value="' + r + '"/>' + (u ? '<a class="edit-comment-cancel">cancel</a>' : "") + '<br/><a class="comment-help-link">help</a></td></tr><tr><td colspan="2"><span class="text-counter"></span><span class="form-error"></span></td></tr></table>';
        x.append(t);
        if (u) {
            x.find(".edit-comment-cancel").click(function () {
                i($(this))
            })
        }
        var w = false;
        var q = function (y) {
            w = y
        };
        var s = x.find("textarea");
        s.charCounter({
            min: 15,
            max: 600,
            setIsValid: q
        });
        x.find(".comment-help-link").click(p);
        if (window.autoShowCommentHelp) {
            s.one("focus", p)
        }
        x.submit(function () {
            if (w) {
                disableSubmitButton(x);
                master.addSpinner(x.find('input[type="submit"]').parent(), {
                    "margin-left": "10px"
                });
                v(x)
            } else {
                x.find("span.text-counter").animate({
                    opacity: 0
                }, 100, function () {
                    $(this).animate({
                        opacity: 1
                    }, 100)
                })
            }
            return false
        });
        master.bind_submitOnEnterPress(x)
    };
    var p = function (v) {
        var w = $(this).closest("tbody");
        var r = w.find(".comment-help-link");
        var x = $(".comment-help", w);
        var u;
        if (v.type == "click") {
            u = x.length == 0 || !x.is(":visible");
            if (!u) {
                p.manualOnly = true
            }
        } else {
            if (p.manualOnly) {
                return
            } else {
                u = true
            }
        }
        var q = function () {
            r.text(u ? "hide help" : "help")
        };
        if (x.length > 0) {
            if (u) {
                x.slideDown(q)
            } else {
                x.slideUp(q)
            }
            return
        }
        if (!u) {
            return
        }
        var s = $("<tr />").appendTo(w);
        var t = $("<td colspan='2' />").appendTo(s);
        if (p.helpText) {
            t.html(p.helpText);
            $(".comment-help", t).slideDown(q)
        } else {
            master.addSpinner(r);
            t.load("/posts/comment-help", function (y) {
                p.helpText = y;
                master.removeSpinner();
                $(".comment-help", this).slideDown(q)
            })
        }
    };
    var n = function (q) {
        l($("#add-comment-" + q), "Add Comment", false, f)
    };
    var f = function (q) {
        var u = q.attr("id").substr("add-comment-".length);
        var s = function () {
            $(".error-notification").fadeOutAndRemove()
        };
        var t = q.find("textarea");
        var r = t.val();
        if (!r || $.trim(r) == "") {
            return
        }
        $.ajax({
            type: "POST",
            url: "/posts/" + u + "/comments",
            dataType: "html",
            data: {
                comment: r,
                fkey: fkey
            },
            success: function (v) {
                s();
                g(u, v);
                t.val("").keyup();
                enableSubmitButton(q);
                q.hide().closest(".comments").siblings('a[id^="comments-link"]').show()
            },
            error: function (v, x, w) {
                s();
                showAjaxError(q, (v.responseText && v.responseText.length < 100 ? v.responseText : "An error occurred during comment submission"));
                enableSubmitButton(q)
            },
            complete: master.removeSpinner
        })
    };
    var h = function (t) {
        var r = t.closest("tr.comment");
        var q = r.find('form[id^="edit-comment-"]');
        r.find("td.comment-actions *").hide();
        r.find("td.comment-text > div").hide();
        r.find("td:last").addClass("comment-form");
        l(q, "Save Edits", true, k);
        var s = q.find("textarea");
        s.val(q.find("div").text());
        q.show();
        s.focus()
    };
    var k = function (w) {
        var r = w.attr("id").substr("edit-comment-".length);
        var v = w.closest("div.comments, td.comment-summary");
        var q = v.attr("id").substr("comments-".length);
        var u = function () {
            $(".error-notification").fadeOut("fast", function () {
                $(this).remove()
            })
        };
        var s = w.find("textarea");
        var t = s.val();
        if (!t || $.trim(t) == "") {
            return
        }
        $.ajax({
            type: "POST",
            url: "/posts/comments/" + r + "/edit",
            dataType: "html",
            data: {
                comment: t,
                fkey: fkey
            },
            success: function (x) {
                u();
                if (e) {
                    v.find("#comment-" + r + " .comment-text .comment-copy").text(t);
                    i(w)
                } else {
                    g(q, x)
                }
            },
            error: function (x, z, y) {
                u();
                showAjaxError(w, (x.responseText && x.responseText.length < 100 ? x.responseText : "An error occurred during comment submission"));
                enableSubmitButton(w)
            },
            complete: master.removeSpinner
        })
    };
    var i = function (s) {
        var r = s.closest("tr.comment");
        var q = r.find('form[id^="edit-comment-"]');
        q.children("table").remove();
        q.hide();
        r.find("td:last").removeClass("comment-form");
        r.find("td.comment-actions *").show();
        r.find("td.comment-text > div").show()
    };
    var m = function (x, r, v, w, t) {
        var u = x.parents("tr.comment").attr("id").substr("comment-".length);
        $("div.error-notification").hide();
        x.removeClass(v).unbind("click mouseenter mouseleave").addClass(w);
        appendLoader(x.parent());
        var q = function () {
            x.removeClass(w).addClass(v).click(function () {
                m(x, r, v, w, t)
            })
        };
        var s = "#comment-" + u + " td.comment-actions";
        $.ajax({
            type: "POST",
            url: "/posts/comments/" + u + "/vote/" + r,
            dataType: "json",
            data: {
                fkey: fkey
            },
            success: function (y) {
                if (y.Success) {
                    t(x, y)
                } else {
                    master.showErrorPopup(s, y.Message);
                    q()
                }
            },
            error: function (y, A, z) {
                master.showErrorPopup(s, (y.responseText && y.responseText.length < 100 ? y.responseText : "An error occurred during voting"));
                q()
            }
        });
        removeLoader()
    };
    return {
        init: function (r, q) {
            e = r;
            c();
            j(q);
            $("form.comment-form-expanded").closest("div.comments").siblings("a.comments-link").click()
        },
        fetch: function (q, r) {
            if (!r) {
                r = d(q)
            }
            master.addSpinner(r);
            $.ajax({
                type: "GET",
                url: "/posts/" + q + "/comments",
                dataType: "html",
                success: function (s) {
                    g(q, s)
                },
                error: function (s, u, t) {
                    showAjaxError(d(q), (s.responseText && s.responseText.length < 100 ? s.responseText : "An error has occured while fetching comments"))
                },
                complete: master.removeSpinner
            })
        }
    }
} ();

function initTagRenderer(b, a) {
    if (window.tagRenderer) {
        return
    }
    window.tagRendererRaw = function (c, f) {
        f = f || "";
        var e = "";
        if (!f) {
            if (b && $.inArray(c, b) > -1) {
                e = " required-tag"
            } else {
                if (a && $.inArray(c, a) > -1) {
                    e = " moderator-tag"
                }
            }
        }
        var d = "<a class='post-tag" + e + "' href='" + f + "/questions/tagged/" + encodeURIComponent(c) + "' title=\"show questions tagged '" + c + "'\" rel='tag'>" + c + "</a>";
        return d
    };
    window.tagRenderer = function (d, c) {
        return $(tagRendererRaw(d, c))
    }
}
function editorReady(b, a) {
    if ($("#show-editor-button").length == 0 && $("#wmd-preview").length != 0) {
        initPostEditor(b, a)
    }
}
function initPostEditor(d, c) {
    var a = $("#post-form");
    var e = $("#wmd-input");
    var b = $("#title, #tagnames, #edit-comment, #m-address");
    a.submit(function () {
        if (window.postSubmitCallback && !window.postSubmitCallback()) {
            return false
        }
        var f = true;
        var g = function (h) {
            f = h
        };
        b.each(function () {
            master.hideHelpOverlay($(this))
        });
        if (f) {
            disableSubmitButton(a);
            setConfirmUnload(null)
        }
        return f
    });
    $("#original-question").not(".processed").TextAreaResizer();
    e.not(".processed").TextAreaResizer();
    e.typeWatch({
        highlight: false,
        wait: 5000,
        captureLength: 5,
        callback: styleCode
    });
    Attacklab.wmdBase();
    Attacklab.Util.startEditor();
    heartbeat.init(d);
    if (d == "ask" || d == "answer") {
        e.add(b).focus(function () {
            master.loadTicks()
        })
    }
    if (c) {
        e.keypress(initNavPrevention)
    }
    $("#wmd-preview").click(function () {
        e.focus()
    });
    master.bindHelpOverlayEvents(b);
    if ($("#ask-error-container").length == 0) {
        $("#title").focus()
    }
}
function initFadingHelpText() {
    var b = {
        "wmd-input": "#how-to-format",
        tagnames: "#how-to-tag",
        title: "#how-to-title"
    };
    var a = $("#wmd-input, #tagnames, #title");
    var c = function (d) {
        return $(b[$(d).attr("id")])
    };
    a.focus(function () {
        a.each(function () {
            c(this).hide()
        });
        c(this).wrap('<div class="dno" />').show().parent().fadeIn("slow", function () {
            $(this).children().unwrap()
        })
    })
}
function initNavPrevention(b) {
    if (b.which == "undefined") {
        return
    }
    var a = $("#wmd-input");
    a.unbind("keypress", initNavPrevention);
    setConfirmUnload("You have started writing or editing a post.", a)
}
var heartbeat = function () {
    var type;
    var jWmd;
    var postId;
    var bindCount = 0;
    var fetchCount = 0;
    var pingInProgress = false;
    var hasBeenNotified = false;
    var rebindHeartbeat = false;
    var hasTimeout = false;
    var timeout = 1000 * 45;
    var notifyMessageTypeId = -2;
    var callbacks = [];
    var initialized = false;
    var submitPing = function (formData, callback) {
        if (callback != null) {
            callbacks.push(callback)
        }
        if (pingInProgress) {
            return
        }
        pingInProgress = true;
        formData.text = jWmd.val();
        rebindHeartbeat = ++fetchCount < 30;
        $.ajax({
            type: "POST",
            url: "/posts/" + postId + "/editor-heartbeat/" + type.name,
            dataType: "json",
            data: formData,
            success: function (json) {
                if (type.pingComplete) {
                    type.pingComplete(json)
                }
                if (json && json.draftSaved) {
                    informDraftSaved()
                }
            },
            error: function () {
                $("#draft-saved").hide()
            },
            complete: function () {
                pingInProgress = false;
                if (rebindHeartbeat && !hasTimeout) {
                    hasTimeout = true;
                    setTimeout(function () {
                        hasTimeout = false;
                        type.ping()
                    }, timeout)
                }
                for (var i = 0; i < callbacks.length; i++) {
                    callbacks[i]()
                }
                callbacks = []
            }
        })
    };
    var informDraftSaved = function () {
        var jDraft = $("#draft-saved");
        var inform = function () {
            jDraft.text("draft saved").fadeIn("fast")
        };
        if (jDraft.is(":visible")) {
            jDraft.fadeOut("fast", inform)
        } else {
            inform()
        }
        var hideDraftSaved = function (event) {
            if (event.which != 115 || !event.ctrlKey || event.shiftKey || event.altKey) {
                jWmd.unbind("keypress", hideDraftSaved);
                $("#draft-saved").fadeOut("fast")
            }
        };
        jWmd.bind("keypress", hideDraftSaved)
    };
    return {
        init: function (heartbeatType) {
            type = (heartbeatType && heartbeatType.length) ? eval("heartbeat." + heartbeatType) : null;
            if (type != null) {
                jWmd = $("#wmd-input");
                jWmd.keypress(heartbeat.keypress)
            }
        },
        ping: function (callback) {
            if (type != null) {
                type.ping(callback)
            }
        },
        draftSaved: function () {
            return (type == null) || !initialized || $("#draft-saved").is(":visible")
        },
        keypress: function (eventArgs) {
            if (eventArgs.which == "undefined") {
                return
            }
            if (bindCount++ > 0) {
                return
            }
            jWmd.unbind("keypress", heartbeat.keypress);
            type.init();
            initialized = true;
            hasTimeout = true;
            setTimeout(function () {
                hasTimeout = false;
                type.ping()
            }, timeout)
        },
        ask: function () {
            return {
                name: "ask",
                init: function () {
                    postId = 0
                },
                ping: function (callback) {
                    var title = $("#title").val();
                    var tagnames = $("#tagnames").val();
                    submitPing({
                        title: title,
                        tagnames: tagnames
                    }, callback)
                }
            }
        } (),
        answer: function () {
            return {
                name: "answer",
                init: function () {
                    postId = location.href.match(/\/questions\/(\d+)/i)[1]
                },
                ping: function (callback) {
                    var clientCount = $("#answers-header .answers-subheader h2").text().replace(/ answers?/i, "") || "0";
                    submitPing({
                        clientCount: clientCount
                    }, callback)
                },
                pingComplete: function (data) {
                    if (data && !hasBeenNotified) {
                        if (data.disableEditor) {
                            var msg = "This question has been " + data.message + " - no more answers will be accepted.";
                            notify.show(msg, notifyMessageTypeId);
                            hasBeenNotified = true;
                            disableSubmitButton("#post-form");
                            setConfirmUnload(null);
                            rebindHeartbeat = false
                        } else {
                            var count = parseInt(data.message);
                            if (count > 0) {
                                var msg2 = count + " new answer" + (count == 1 ? " has" : "s have") + " been posted - ";
                                msg2 += '<a onclick="heartbeat.answer.update()">load new answers.</a>';
                                notify.show(msg2, notifyMessageTypeId);
                                hasBeenNotified = true
                            }
                        }
                    }
                },
                update: function () {
                    var divIdsToAdd = [];
                    $.get("/questions/" + postId, function (html) {
                        var jHtml = $(html);
                        jHtml.find("div.answer").each(function () {
                            var id = this.id.substring("answer-".length);
                            if ($("#answer-" + id).length == 0) {
                                divIdsToAdd.push(this.id)
                            }
                        });
                        if (divIdsToAdd.length > 0) {
                            var selector = "#" + divIdsToAdd.join(",#");
                            var divs = jHtml.find(selector);
                            var appendAfter = $("div.answer:last");
                            if (appendAfter.length == 0) {
                                appendAfter = $("#answers-header")
                            }
                            divs.hide();
                            appendAfter.after(divs);
                            divs.fadeIn("slow");
                            var totalAnswers = $("div.answer").length;
                            $(".subheader h2").text(totalAnswers + " Answer" + (totalAnswers > 1 ? "s" : ""));
                            vote.init(postId);
                            comments.init()
                        }
                        notify.close(notifyMessageTypeId);
                        hasBeenNotified = false
                    }, "html")
                }
            }
        } (),
        edit: function () {
            return {
                name: "edit",
                init: function () {
                    postId = $("#post-id").val()
                },
                ping: function (callback) {
                    submitPing({
                        clientRevisionGuid: $("#client-revision-guid").val()
                    }, callback)
                },
                pingComplete: function (json) {
                    if (json && !hasBeenNotified) {
                        if (json.message) {
                            if (notify.getMessageText(notifyMessageTypeId) != json.message) {
                                notify.close(notifyMessageTypeId);
                                notify.show(json.message, notifyMessageTypeId)
                            }
                            setConfirmUnload(null);
                            if (json.disableEditor) {
                                disableSubmitButton("#post-form");
                                rebindHeartbeat = false
                            }
                        }
                    }
                }
            }
        } ()
    }
} ();
var sedits = function () {
    var c = {};
    var h = function (i) {
        return $(i).closest(".edit-suggestion").attr("id").substring("edit-suggestion-".length)
    };
    var f = function (j) {
        var i = j.find(".votecell .vote input").val();
        return i
    };
    var a = function (i, j) {
        var l = h(i);
        var k = i.closest("form");
        if (k.data("working")) {
            return
        }
        k.data("working", true).find("input[type=button]").disable();
        i.addSpinnerAfter({
            padding: "0 3x"
        });
        $.ajax({
            url: "/edit-suggestion/{editId}/vote/{type}".format({
                editId: l,
                type: j
            }),
            data: {
                fkey: fkey
            },
            dataType: "json",
            type: "POST",
            success: function (m) {
                g(k, i, j, m)
            },
            error: function (m, n, o) {
                d(k, i, "An error has occurred when trying to " + j + " - please try again")
            },
            complete: master.removeSpinner
        })
    };
    var g = function (i, j, k, l) {
        if (!l.Success) {
            d(i, j, l.Message);
            return
        }
        if (c.popup) {
            if (k == "approve") {
                location.href = l.RedirectTo
            }
            $("#lightbox, #lightbox-panel").fadeOutAndRemove()
        } else {
            if (c.refreshPageOnActionSuccess) {
                location.reload(true)
            } else {
                e(j)
            }
        }
    };
    var e = function (i) {
        i.closest(".edit-suggestion").fadeOutAndRemove("slow");
        b()
    };
    var b = function () {
        $("#hlinks-nav .mod-flag-indicator.hotbg, #tabs .youarehere .mod-flag-indicator.hotbg").each(function () {
            var i = $(this);
            i.text((i.text() || 0) - 1);
            if (i.text() < 1) {
                i.hide()
            }
        })
    };
    var d = function (i, k, j) {
        master.removeSpinner();
        i.find("input[type=button]").not(k).fadeOut("fast");
        k.fadeOut("fast", function () {
            if (!c.hideRefreshButton) {
                $('<input type="button" class="refresh-button" onclick="location.reload(true)" value="Refresh Page" style="display:none">').appendTo(i).fadeIn("fast")
            }
            i.closest("table").find(".form-error").html(j).fadeIn("fast", function () {
                if (c.popup) {
                    var l = i.closest(".popup-edit-suggestion");
                    if (l.scrollTop() > 0) {
                        l.scrollTop(l.find(".edit-suggestion").height())
                    }
                }
            })
        })
    };
    return {
        init: function (i) {
            c = i || {};
            $(".approve-edit").unbind("click").click(function () {
                a($(this), "approve");
                return false
            });
            $(".reject-edit").unbind("click").click(function () {
                a($(this), "reject");
                return false
            });
            $(".edit-suggestion .action:not(.link)").unbind("click").click(function () {
                var k = $(this);
                var l = k.hasClass("full-diff") ? "full-diff" : "full-html-diff";
                var j = k.closest(".edit-suggestion").find(".body-diffs");
                j.find("table:visible").hide();
                j.find("." + l).show();
                k.siblings(".action.selected").removeClass("selected");
                k.addClass("selected");
                return false
            });
            vote.init_delete(function (j, m, l) {
                if (l && l.Success) {
                    e(j)
                } else {
                    var k = (l && l.Message) ? l.Message : "A problem occurred during deletion";
                    j.parent().showErrorPopup(k)
                }
            })
        },
        bindPopupLinks: function () {
            $("div.post-menu a[id^='edit-pending-']").unbind("click").click(function () {
                sedits.loadPopup($(this));
                return false
            })
        },
        loadPopup: function (j) {
            if (j.data("working")) {
                return
            }
            j.data("working", true).addSpinnerAfter({
                padding: "3px 0"
            });
            var i = j.parent();
            var k = j.attr("id").substring("edit-pending-".length);
            $.ajax({
                type: "GET",
                url: "/suggested-edits/popup/" + k,
                dataType: "html",
                success: function (m) {
                    var o = $('<div id="lightbox-panel" class="popup" style="display:block"><div class="popup-close"><a title="close this popup (or hit Esc)">&times;</a></div><div class="popup-edit-suggestion">' + m + "</div></div>");
                    var l = $(window).height();
                    var n = o.find(".popup-edit-suggestion").css({
                        "max-height": l - 100
                    });
                    $('<div id="lightbox"/>').appendTo($("body")).css("height", $(document).height()).fadeIn("fast");
                    o.appendTo(i).center().fadeIn("fast").find(".popup-close").click(function () {
                        $("#lightbox, #lightbox-panel").fadeOutAndRemove()
                    });
                    sedits.init({
                        popup: true
                    })
                },
                error: function (l, n, m) {
                    i.showErrorPopup(l.responseText && l.responseText.length < 200 ? l.responseText : "Unable to load suggested edit - please try again")
                },
                complete: function () {
                    master.removeSpinner();
                    j.removeData("working")
                }
            });
            return false
        }
    }
} ();
(function (a) {
    a.fn.typeWatch = function (b) {
        var e = a.extend({
            wait: 750,
            callback: function () { },
            highlight: true,
            captureLength: 2
        }, b);

        function c(f, g) {
            var h = a(f.el).val();
            if ((h.length > e.captureLength && h.toUpperCase() != f.text) || (g && h.length > e.captureLength)) {
                f.text = h.toUpperCase();
                f.cb(h)
            }
        }
        function d(h) {
            if (h.type.toUpperCase() == "TEXT" || h.nodeName.toUpperCase() == "TEXTAREA") {
                var f = {
                    timer: null,
                    text: a(h).val().toUpperCase(),
                    cb: e.callback,
                    el: h,
                    wait: e.wait
                };
                if (e.highlight) {
                    a(h).focus(function () {
                        this.select()
                    })
                }
                var g = function (l) {
                    var j = f.wait;
                    var k = false;
                    if (l.keyCode == 13 && this.type.toUpperCase() == "TEXT") {
                        j = 1;
                        k = true
                    }
                    var i = function () {
                        c(f, k)
                    };
                    clearTimeout(f.timer);
                    f.timer = setTimeout(i, j)
                };
                a(h).keydown(g)
            }
        }
        return this.each(function (f) {
            d(this)
        })
    }
})(jQuery);
(function (f) {
    var i, g;
    var c = 0;
    var h = 32;
    var b;
    f.fn.TextAreaResizer = function () {
        return this.each(function () {
            i = f(this).addClass("processed");
            g = null;
            f(this).parent().append(f('<div class="grippie"></div>').bind("mousedown", {
                el: this
            }, j));
            var k = f("div.grippie", f(this).parent())[0];
            k.style.marginRight = (k.offsetWidth - f(this)[0].offsetWidth) + "px"
        })
    };

    function j(k) {
        i = f(k.data.el);
        i.blur();
        c = d(k).y;
        g = i.height() - c;
        i.css("opacity", 0.25);
        f(document).mousemove(e).mouseup(a);
        return false
    }
    function e(m) {
        var k = d(m).y;
        var l = g + k;
        if (c >= (k)) {
            l -= 5
        }
        c = k;
        l = Math.max(h, l);
        i.height(l + "px");
        if (l < h) {
            a(m)
        }
        return false
    }
    function a(k) {
        f(document).unbind("mousemove", e).unbind("mouseup", a);
        i.css("opacity", 1);
        i.focus();
        i = null;
        g = null;
        c = 0
    }
    function d(k) {
        return {
            x: k.clientX + document.documentElement.scrollLeft,
            y: k.clientY + document.documentElement.scrollTop
        }
    }
})(jQuery);
(function (d) {
    d.each(["backgroundColor", "borderBottomColor", "borderLeftColor", "borderRightColor", "borderTopColor", "color", "outlineColor"], function (f, e) {
        d.fx.step[e] = function (g) {
            if (!g.colorInit) {
                g.start = c(g.elem, e);
                g.end = b(g.end);
                g.colorInit = true
            }
            g.elem.style[e] = "rgb(" + [Math.max(Math.min(parseInt((g.pos * (g.end[0] - g.start[0])) + g.start[0]), 255), 0), Math.max(Math.min(parseInt((g.pos * (g.end[1] - g.start[1])) + g.start[1]), 255), 0), Math.max(Math.min(parseInt((g.pos * (g.end[2] - g.start[2])) + g.start[2]), 255), 0)].join(",") + ")"
        }
    });

    function b(e) {
        var f;
        if (e && e.constructor == Array && e.length == 3) {
            return e
        }
        if (f = /rgb\(\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*\)/.exec(e)) {
            return [parseInt(f[1]), parseInt(f[2]), parseInt(f[3])]
        }
        if (f = /rgb\(\s*([0-9]+(?:\.[0-9]+)?)\%\s*,\s*([0-9]+(?:\.[0-9]+)?)\%\s*,\s*([0-9]+(?:\.[0-9]+)?)\%\s*\)/.exec(e)) {
            return [parseFloat(f[1]) * 2.55, parseFloat(f[2]) * 2.55, parseFloat(f[3]) * 2.55]
        }
        if (f = /#([a-fA-F0-9]{2})([a-fA-F0-9]{2})([a-fA-F0-9]{2})/.exec(e)) {
            return [parseInt(f[1], 16), parseInt(f[2], 16), parseInt(f[3], 16)]
        }
        if (f = /#([a-fA-F0-9])([a-fA-F0-9])([a-fA-F0-9])/.exec(e)) {
            return [parseInt(f[1] + f[1], 16), parseInt(f[2] + f[2], 16), parseInt(f[3] + f[3], 16)]
        }
        if (f = /rgba\(0, 0, 0, 0\)/.exec(e)) {
            return a.transparent
        }
        return a[d.trim(e).toLowerCase()]
    }
    function c(g, f) {
        var e;
        do {
            e = d.curCSS(g, f);
            if (e != "" && e != "transparent" || d.nodeName(g, "body")) {
                break
            }
            f = "backgroundColor"
        } while (g = g.parentNode);
        return b(e)
    }
    var a = {}
})(jQuery);